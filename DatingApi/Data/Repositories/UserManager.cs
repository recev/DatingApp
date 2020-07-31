using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DatingApi.Data.Models;

namespace DatingApi.Data.Repositories
{
    public class UserManager : IUserManager
    {
        DatingDbContext _context;
        public UserManager(DatingDbContext context)
        {
            this._context = context;
        }
        public IList<User> GetAllUsers()
        {
            IList<User> users = null;

            try
            {
                users = _context.Users.ToList();
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

        public User FindUser(string userName)
        {
            User user = null;
            try
            {
                user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
             
            return user;
        }

        public bool DoesUserExist(string userName)
        {
            var result = false;

            try
            {
                var user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if(user != null)
                    result = true;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return result;
        }
    }
}