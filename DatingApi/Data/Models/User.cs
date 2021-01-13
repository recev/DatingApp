using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DatingApi.Data.Models
{
    public class User: IdentityUser
    {
        [Required]
        [MinLength(4)]
        [MaxLength(32)]
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string KnownAs { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public byte[] PasswordSaltKey { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Like> SendedLikes { get; set; }
        public ICollection<Like> ReceivedLikes { get; set; }
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}