using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.OperationResults;
using Microsoft.AspNetCore.Http;

namespace DatingApi.Data.Repositories
{
    public interface IPhotoRepository
    {
        PhotoForClient GetPhoto(int userId, int photoId);
        UploadPhotoResult UploadPhoto(int userId, IFormFile imageFile);
        OperationResult SetMainPhoto(int userId, int photoId);
        OperationResult DeletePhoto(int userId, int photoId);
    }
}