using DatingApi.Data.DataTransferObjects;

namespace DatingApi.Data.OperationResults
{
    public class UploadPhotoResult
    {
        public bool IsSuccessful { get; set; }   
        public string Message { get; set; }
        public PhotoForClient Photo { get; set; }
    }
}