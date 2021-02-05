using System;

namespace DatingApi.Data.DataTransferObjects
{
    public class PhotoForUser
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string KnownAs { get; set; }
        public int PhotoId { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public bool IsApproved { get; set; }
    }
}
