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
                )
                .ForMember(user => user.Roles, 
                    configuration => configuration.MapFrom(
                        user => user.UserRoles.Select(userRole => userRole.Role.Name)
                    )
                );

            CreateMap<User, RegisterUser>();
            CreateMap<RegisterUser, User>();

            CreateMap<Photo, PhotoForClient>();

            CreateMap<UpdateUser, User>();

            CreateMap<User, UserRoleDto>()
                .ForMember(user => user.Roles,
                    conf => conf.MapFrom(userRole =>
                                userRole.UserRoles.Select(ur => ur.Role))
                );

            CreateMap<Message, MessageDto>()
            .ForMember(
                p => p.SenderPhotoUrl, 
                opt => opt.MapFrom(m => m.Sender.Photos.FirstOrDefault(p => p.IsMain == true).Url))
            .ForMember(
                p => p.RecipientPhotoUrl, 
                opt => opt.MapFrom(m => m.Recipient.Photos.FirstOrDefault(p => p.IsMain == true).Url));

            CreateMap<UserMessage, Message>();


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