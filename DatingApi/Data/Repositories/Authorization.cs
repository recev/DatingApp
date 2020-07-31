using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Data;
using DatingApi.Data.Models;
using System.Security.Cryptography;

namespace DatingApi.Data.Repositories
{
    public class Authorization : IAuthorization
    {
        DatingDbContext _context;
        public Authorization(DatingDbContext context)
        {
            this._context = context;
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

        public User Login(string userName, string password)
        {
            var user = FindUser(userName);

            AreCredentialsValid(user, password);

            GenerateToken(user);

            return user;
        }

        private string GenerateToken(User user)
        {
            string token = "";


            return token;
        }

        private User FindUser(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            return user;
        }

        private bool AreCredentialsValid(User user, string password)
        {
            var result = true;
            try
            {
                using (var hmac = new HMACSHA512(user.PasswordSaltKey))
                {
                    var passwordInBytes = System.Text.Encoding.UTF8.GetBytes(password);

                    var passwordHash = hmac.ComputeHash(passwordInBytes);

                    if (passwordHash == user.PasswordHash)
                        result = true;
                }   
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return result;
        }

        public bool Register(string userName, string Password)
        {
            var result = false;

            var user = CreateUser(userName, Password);

            if(user == null)
                return result;

            result = SaveUser(user);
            
            return result;
        }

        private User CreateUser(string userName, string Password)
        {
            User user = null;
            try
            {
                using (var hcmac = new HMACSHA512())
                {

                    var passwordInBytes = System.Text.Encoding.UTF8.GetBytes(Password);
                    var passwordHash = hcmac.ComputeHash(passwordInBytes);
                    
                    user = new User {
                        UserName = userName,
                        PasswordSaltKey = hcmac.Key,
                        PasswordHash = passwordHash
                    };
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return user;
        }

        private bool SaveUser(User user)
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
    }
}