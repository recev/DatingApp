using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.OperationResults;
using Microsoft.AspNetCore.Http;

namespace DatingApi.Data.Repositories
{
    public interface IPhotoRepository
    {
        PhotoForClient GetPhoto(string userId, int photoId);
        UploadPhotoResult UploadPhoto(string userId, IFormFile imageFile);
        OperationResult SetMainPhoto(string userId, int photoId);
        OperationResult DeletePhoto(string userId, int photoId);
    }
}