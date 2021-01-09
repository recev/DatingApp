using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatingApi.Data.Models
{
    public class Message
    {
        public int Id { get; set; }
    
        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int RecipientId { get; set; }
        public User Recipient { get; set; }

        public DateTime SentDate { get; set; }
        public DateTime ReadDate { get; set; }

        public bool IsRead { get; set; }
        public bool IsSenderDeleted { get; set; }
        public bool IsRecipientDeleted { get; set; }

        public string Content { get; set; }
    }
}