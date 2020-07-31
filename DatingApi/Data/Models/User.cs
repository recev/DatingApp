using System.ComponentModel.DataAnnotations;

namespace DatingApi.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [MinLength(4)]
        [MaxLength(32)]
        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSaltKey { get; set; }
    }
}