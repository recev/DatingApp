using System;

namespace DatingApi.Data.DataTransferObjects
{
    public class MessageDto
    {
        public int Id { get; set; }
    
        public int SenderId { get; set; }
        public string SenderKnownAs { get; set; }
        public string SenderPhotoUrl { get; set; }

        public int RecipientId { get; set; }
        public string RecipientKnownAs { get; set; }
        public string RecipientPhotoUrl { get; set; }

        public DateTime SentDate { get; set; }
        public DateTime ReadDate { get; set; }

        public bool IsRead { get; set; }
        public bool IsSenderDeleted { get; set; }
        public bool IsRecipientDeleted { get; set; }

        public string Content { get; set; }
    }
}