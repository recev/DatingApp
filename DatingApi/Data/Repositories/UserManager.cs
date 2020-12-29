using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Data;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DatingApi.Data.Repositories
{
    public class UserManager : IUserManager
    {
        DatingDbContext _context;
        IMapper _mapper;
        ILogger<UserManager> _logger;
        
        public UserManager(DatingDbContext context, IMapper mapper, ILogger<UserManager> logger)
        {
            this._context = context;
            this._mapper = mapper;
            this._logger = logger;
        }

        public DetailedUser GetUserDetails(int id)
        {
            DetailedUser user = null;

            try
            {
                var dbUser = _context.Users
                    .Include(user => user.Photos)
                    .FirstOrDefault(user => user.Id == id);
                    
                user = _mapper.Map<DetailedUser>(dbUser);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return user;
        }

        public DetailedUser GetUserDetails(string username)
        {
            DetailedUser detailedUser = null;
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == username);
                detailedUser = _mapper.Map<DetailedUser>(user);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }
             
            return detailedUser;
        }

        public PaginatedUserList GetUserList(SearchUser searchUser)
        {
            PaginatedUserList paginatedUserList = null;

            try
            {
                var query = _context.Users.Include(user => user.Photos).AsQueryable();

                if(searchUser.OrderBy.ToLowerInvariant() == "created")
                    query = query.OrderByDescending(u => u.Created);
                else
                    query = query.OrderByDescending(u => u.LastActive);

                var maxAge = DateTime.Now.AddYears(-searchUser.MinAge - 1);
                var minAge = DateTime.Now.AddYears(-searchUser.MaxAge - 1);

                query = query.Where(u => u.DateOfBirth >= minAge && u.DateOfBirth <= maxAge);

                if(searchUser.Gender != "all")
                    query = query.Where(q => q.Gender == searchUser.Gender);

                var userCount = query.Count();

                var itemsToSkip = (searchUser.PageNumber - 1) * searchUser.PageSize;
                query = query.Skip(itemsToSkip);
                query = query.Take(searchUser.PageSize);

                var UserList = _mapper.Map<IList<CompactUser>>(query.ToList());

                paginatedUserList = new PaginatedUserList{
                    PageNumber = searchUser.PageNumber,
                    PageSize = searchUser.PageSize,
                    TotalUserCount = userCount,
                    Users = UserList
                };
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            
            return paginatedUserList;
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
                _logger.LogError(ex.Message);
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
                _logger.LogError(ex.Message);
            }
             
            return user;
        }

        public User FindUser(int userId)
        {
            User user = null;
            try
            {
                user = _context.Users.FirstOrDefault(u => u.Id == userId);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
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
                _logger.LogError(ex.Message);
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
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public bool UpdateUser(UpdateUser updateUser)
        {
            bool result = false;

            try
            {
                var dbUser = FindUser(updateUser.Username);

                if(dbUser == null)
                    return result;

                var user = _mapper.Map(updateUser, dbUser);

                _context.SaveChanges();
                
                result = true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public bool UpdateLastActive(int userId)
        {
            bool result = false;

            try
            {
                var dbUser = FindUser(userId);

                if(dbUser == null)
                    return result;

                dbUser.LastActive = System.DateTime.Now;

                _context.SaveChanges();

                result = true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }
    }
}