using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.OperationResults;

namespace DatingApi.Data.Repositories
{
    public interface IPhotoRepository
    {
        PhotoForClient GetPhoto(int userId, int photoId);
        UploadPhotoResult UploadPhoto(UploadPhoto image);
        bool SetMainPhoto(int userId, int photoId);
        OperationResult DeletePhoto(int userId, int photoId);
    }
}