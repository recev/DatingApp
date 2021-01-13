using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DatingApi.Data.Models
{
    public class Role: IdentityRole
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public string Description { get; set; }
    }
}