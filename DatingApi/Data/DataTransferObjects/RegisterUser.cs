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
        [MinLength(8)]
        [MaxLength(32)]
        public string Password { get; set; }
    }
}