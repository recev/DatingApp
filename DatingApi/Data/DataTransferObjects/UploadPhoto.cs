using System.IO;
using Microsoft.AspNetCore.Http;

namespace DatingApi.Data.DataTransferObjects
{
    public class UploadPhoto
    {
        public int UserId { get; set; }
        public  IFormFile File { get; set; }
    }
}