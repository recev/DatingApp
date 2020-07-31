using System.ComponentModel.DataAnnotations;

namespace DatingApi.Data.DataTransferObjects
{
    public class LoginUser
    {
        [Required]
        [MinLength(4)]
        [MaxLength(32)]
        public string UserName { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(32)]
        public string Password { get; set; }
    }
}