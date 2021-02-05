using System;

namespace DatingApi.Data.DataTransferObjects
{
    public class PhotoForClient
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime AddedDate { get; set; }
        public Boolean IsMain { get; set; }
        public string PublicId { get; set; }
        public string UserId { get; set; }
        public bool IsApproved { get; set; }
    }
}
