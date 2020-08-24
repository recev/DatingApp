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

namespace DatingApi.Data.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        CloudinarySettings _cloudinarySettings;
        DatingDbContext _context;
        IMapper _mapper;

        public PhotoRepository(IOptions<CloudinarySettings> cloudinarySettingOptions, DatingDbContext context, IMapper mapper)
        {
            this._cloudinarySettings = cloudinarySettingOptions.Value;
            this._context = context;
            this._mapper = mapper;
        }

        public PhotoForClient GetPhoto(int userId, int photoId)
        {
            PhotoForClient clientPhoto = null;
            try
            {
                var dbPhoto = FindPhoto(userId, photoId);
                clientPhoto = _mapper.Map<PhotoForClient>(dbPhoto);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return clientPhoto;
        }

        public Photo FindPhoto(int userId, int photoId)
        {
            Photo photo = null;
            try
            {
                photo = _context.Photos.Include(p => p.User).FirstOrDefault(p => p.UserId == userId && p.Id == photoId);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return photo;
        }

        public UploadPhotoResult UploadPhoto(UploadPhoto uploadPhoto)
        {
            var result = new UploadPhotoResult();
            var uploadResult = UploadImageToCloudinary(uploadPhoto);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                result.Message = "Photo Can not be uploaded to Cloudinary!";
                return result;
            }

            var isMainPhoto = IsMainPhoto(uploadPhoto);

            var newPhoto = new Photo()
            {
                UserId = uploadPhoto.UserId,
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString(),
                IsMain = isMainPhoto,
                AddedDate = DateTime.Now
            };

            if (SavePhoto(newPhoto))
            {
                result.IsSuccessful = true;
                result.Photo = _mapper.Map<PhotoForClient>(newPhoto);
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
                System.Console.WriteLine(ex.Message);
            }

            return result;
        }

        private bool IsMainPhoto(UploadPhoto image)
        {
            var isMainPhoto = true;

            var mainPhoto = _context.Photos.FirstOrDefault(p => p.User.Id == image.UserId && p.IsMain);
            if (mainPhoto != null)
                isMainPhoto = false;
            return isMainPhoto;
        }

        private ImageUploadResult UploadImageToCloudinary(UploadPhoto image)
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

                using (var imageStream = image.File.OpenReadStream())
                {
                    var cloudinary = new Cloudinary(account);

                    uploadResult = cloudinary.Upload(new ImageUploadParams
                    {
                        File = new FileDescription(image.File.Name, imageStream)
                    });
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
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
                System.Console.WriteLine(ex.Message);
            }

            return deletionResult;
        }

        public bool SetMainPhoto(int userId, int photoId)
        {
            bool result = false;

            try
            {
                var dbPhoto = FindPhoto(userId, photoId);

                if (dbPhoto == null)
                    return result;

                var mainPhotoInDb = GetMainPhotoOfUser(userId);

                if (mainPhotoInDb?.Id == photoId)
                    return result;

                mainPhotoInDb.IsMain = false;
                dbPhoto.IsMain = true;

                _context.SaveChanges();

                result = true;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return result;
        }

        private Photo GetMainPhotoOfUser(int userId)
        {
            Photo photo = null;

            try
            {
                photo = _context.Photos.Include(p => p.User)
                    .FirstOrDefault(p => p.UserId == userId && p.IsMain == true);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return photo;
        }

        public OperationResult DeletePhoto(int userId, int photoId)
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
                System.Console.WriteLine(ex.Message);
            }

            return result;
        }
    }
}