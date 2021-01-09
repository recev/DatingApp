using System;

namespace DatingApi.Data.DataTransferObjects
{
    public class UserMessage
    {
        public int Id { get; set; }
    
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime SentDate { get; set; }
        public string Content { get; set; }

        public UserMessage()
        {
            SentDate = DateTime.Now;
        }
    }
}