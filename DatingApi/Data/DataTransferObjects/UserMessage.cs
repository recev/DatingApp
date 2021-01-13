using System;

namespace DatingApi.Data.DataTransferObjects
{
    public class UserMessage
    {
        public int Id { get; set; }
    
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public DateTime SentDate { get; set; }
        public string Content { get; set; }

        public UserMessage()
        {
            SentDate = DateTime.Now;
        }
    }
}