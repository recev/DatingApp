using System;

namespace DatingApi.Data.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime AddedDate { get; set; }
        public Boolean IsMain { get; set; }
        public string PublicId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}