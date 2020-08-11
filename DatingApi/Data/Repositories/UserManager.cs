using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Data;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApi.Data.Repositories
{
    public class UserManager : IUserManager
    {
        DatingDbContext _context;
        IMapper _mapper;
        
        public UserManager(DatingDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public UserForDetail GetUserDetails(int id)
        {
            UserForDetail user = null;

            try
            {
                var dbUser = _context.Users
                    .Include(user => user.Photos)
                    .FirstOrDefault(user => user.Id == id);
                    
                user = _mapper.Map<UserForDetail>(dbUser);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return user;
        }
        
        public IList<UserForList> GetUserList()
        {
            IList<UserForList> users = null;

            try
            {
                var dbUsers = _context.Users.Include(user => user.Photos).ToList();
                users = _mapper.Map<IList<UserForList>>(dbUsers);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            
            return users;
        }

        public bool SaveUser(User user)
        {
            var result = false;

            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                result = true;   
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return result;
        }

        public User FindUser(string username)
        {
            User user = null;
            try
            {
                user = _context.Users.FirstOrDefault(u => u.Username == username);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
             
            return user;
        }

        public bool DoesUserExist(string username)
        {
            var result = false;

            try
            {
                var user = _context.Users.Where(u => u.Username == username).FirstOrDefault();

                if(user != null)
                    result = true;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return result;
        }
    
        public bool DeleteUser(string userName)
        {
            bool result = false;

            try
            {
                var foundUser = _context.Users.Where(u => u.Username == userName).FirstOrDefault();

                if(foundUser != null)
                {
                    _context.Users.Remove(foundUser);
                    _context.SaveChanges();
                    result = true;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }
    
    }
}