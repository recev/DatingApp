using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApi.Data.DataTransferObjects
{
    public class RegisterUser
    {
        [Required]
        [MinLength(4)]
        [MaxLength(32)]
        public string Username { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(32)]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }

        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required]
        public string Country { get; set; }
        
        [Required]
        public string KnownAs { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}