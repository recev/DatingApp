using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DatingApi.Data.Models;

namespace DatingApi.Data.DataTransferObjects
{
    public class DetailedUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string KnownAs { get; set; }
        public int Age { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotoForClient> Photos { get; set; }        
    }
}