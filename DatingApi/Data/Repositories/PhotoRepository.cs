using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApi.Data.DataTransferObjects;
using Microsoft.Extensions.Options;
using DatingApi.Settings;
using Data;
using System.Linq;
using DatingApi.Data.OperationResults;
using DatingApi.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DatingApi.Data.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        CloudinarySettings _cloudinarySettings;
        DatingDbContext _context;
        IMapper _mapper;
        ILogger<PhotoRepository> _logger;

        public PhotoRepository(IOptions<CloudinarySettings> cloudinarySettingOptions, DatingDbContext context, IMapper mapper, ILogger<PhotoRepository> logger)
        {
            this._cloudinarySettings = cloudinarySettingOptions.Value;
            this._context = context;
            this._mapper = mapper;
            this._logger = logger;
        }

        public PhotoForClient GetPhoto(string userId, int photoId)
        {
            PhotoForClient clientPhoto = null;
            try
            {
                var dbPhoto = FindPhoto(userId, photoId);
                clientPhoto = _mapper.Map<PhotoForClient>(dbPhoto);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return clientPhoto;
        }

        public Photo FindPhoto(string userId, int photoId)
        {
            Photo photo = null;
            try
            {
                photo = _context.Photos.Include(p => p.User).FirstOrDefault(p => p.UserId == userId && p.Id == photoId);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return photo;
        }

        public OperationResult<PhotoForClient> UploadPhoto(string userId, IFormFile file)
        {
            var result = new OperationResult<PhotoForClient>();
            var uploadResult = UploadImageToCloudinary(userId, file);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                result.Message = "Photo Can not be uploaded to Cloudinary!";
                return result;
            }

            var isMainPhoto = IsMainPhoto(userId);

            var newPhoto = new Photo()
            {
                UserId = userId,
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString(),
                IsMain = isMainPhoto,
                AddedDate = DateTime.Now
            };

            if (SavePhoto(newPhoto))
            {
                result.IsSuccessful = true;
                result.Value = _mapper.Map<PhotoForClient>(newPhoto);
            }

            return result;
        }

        private bool SavePhoto(Photo newPhoto)
        {
            bool result = false;
            try
            {
                _context.Photos.Add(newPhoto);
                _context.SaveChanges();
                result = true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }

        private bool IsMainPhoto(string userId)
        {
            var isMainPhoto = true;

            var mainPhoto = _context.Photos.FirstOrDefault(p => p.User.Id == userId && p.IsMain);
            if (mainPhoto != null)
                isMainPhoto = false;
            return isMainPhoto;
        }

        private ImageUploadResult UploadImageToCloudinary(string userId, IFormFile imageFile)
        {
            ImageUploadResult uploadResult = null;
            try
            {
                var account = new Account
                {
                    ApiKey = _cloudinarySettings.ApiKey,
                    ApiSecret = _cloudinarySettings.ApiSecret,
                    Cloud = _cloudinarySettings.CloudName
                };

                using (var imageStream = imageFile.OpenReadStream())
                {
                    var cloudinary = new Cloudinary(account);
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(imageFile.Name, imageStream)
                    };

                    uploadResult = cloudinary.Upload(uploadParams);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return uploadResult;
        }

        private DeletionResult DeleteImageFromCloudinary(string publicId)
        {
            DeletionResult deletionResult = null;
            try
            {
                var account = new Account
                {
                    ApiKey = _cloudinarySettings.ApiKey,
                    ApiSecret = _cloudinarySettings.ApiSecret,
                    Cloud = _cloudinarySettings.CloudName
                };

                var cloudinary = new Cloudinary(account);
                var deletionParameters = new DeletionParams(publicId);

                deletionResult = cloudinary.Destroy(deletionParameters);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return deletionResult;
        }

        public OperationResult SetMainPhoto(string userId, int photoId)
        {
            var result = new OperationResult();

            try
            {
                var dbPhoto = FindPhoto(userId, photoId);

                if (dbPhoto == null)
                {
                    result.Message = "Photo not found!";
                    return result;
                }

                var mainPhotoInDb = GetMainPhotoOfUser(userId);

                if (mainPhotoInDb?.Id == photoId)
                {
                    result.Message = "It's already main photo!";
                    return result;
                }

                dbPhoto.IsMain = true;

                if(mainPhotoInDb != null)
                {
                    mainPhotoInDb.IsMain = false;
                }

                _context.SaveChanges();

                result.IsSuccessful = true;
                return result;
            }
            catch (System.Exception ex)
            {
                result.Message = ex.Message;
                _logger.LogError(ex.Message);
            }

            return result;
        }

        private Photo GetMainPhotoOfUser(string userId)
        {
            Photo photo = null;

            try
            {
                photo = _context.Photos.Include(p => p.User)
                    .FirstOrDefault(p => p.UserId == userId && p.IsMain == true);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return photo;
        }

        public OperationResult DeletePhoto(string userId, int photoId)
        {
            var result = new OperationResult();

            var dbPhoto = FindPhoto(userId, photoId);

            if(dbPhoto == null)
            {
                result.Message = "Photo can not be found!";
                return result;
            }

            if (dbPhoto.IsMain)
            {
                result.Message = "You can not delete the main photo!";
                return result;
            }

            var deletionResult = DeleteImageFromCloudinary(dbPhoto.PublicId);

            if (deletionResult.StatusCode != HttpStatusCode.OK)
            {
                result.Message = "Image can not be deleted!";
                return result;
            }

            if(!DeletePhotoFromDb(dbPhoto))
            {
                result.Message = "Image can not be deleted from db!";
                return result;
            }

            result.Message = "Image deleted successful!";
            result.IsSuccessful = true;
            
            return result;
        }

        private bool DeletePhotoFromDb(Photo dbPhoto)
        {
            bool result = false;

            try
            {
                _context.Entry<Photo>(dbPhoto).State = EntityState.Deleted;
                _context.SaveChanges();
                result = true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }
    }
}