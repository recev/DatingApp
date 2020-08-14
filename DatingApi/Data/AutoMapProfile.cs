using System.Linq;
using System;
using AutoMapper;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.Models;

namespace DatingApi.Data
{
    public class AutoMapProfile: Profile
    {
        public AutoMapProfile()
        {
            CreateMap<User, CompactUser>()
                .ForMember(destination => destination.Age, 
                    configuration => configuration.MapFrom(user => GetAge(user.DateOfBirth))
                )
                .ForMember(destination => destination.PhotoUrl, 
                    configuration => configuration.MapFrom(user =>
                        user
                        .Photos
                        .FirstOrDefault(photo => photo.IsMain)
                        .Url
                    )
                );


            CreateMap<User, DetailedUser>()
                .ForMember(destination => destination.Age, 
                    configuration => configuration.MapFrom(user => GetAge(user.DateOfBirth))
                )
                .ForMember(destination => destination.PhotoUrl, 
                    configuration => configuration.MapFrom(user =>
                        user
                        .Photos
                        .FirstOrDefault(photo => photo.IsMain)
                        .Url
                    )
                );

            CreateMap<Photo, PhotoForClient>();
        }

        private int GetAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;

            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }
}