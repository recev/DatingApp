using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.Models;
using DatingApi.Data.OperationResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DatingApi.Data.Repositories
{
    public class userRepository : IUserRepository
    {
        DatingDbContext _context;
        IMapper _mapper;
        ILogger<userRepository> _logger;
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        IAuthorization _authorization;

        public userRepository(
            DatingDbContext context,
            IMapper mapper,
            ILogger<userRepository> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IAuthorization authorization)
        {
            this._signInManager = signInManager;
            this._context = context;
            this._mapper = mapper;
            this._logger = logger;
            this._userManager = userManager;
            this._authorization = authorization;
        }

        public DetailedUser GetUserDetailsByUserId(string id)
        {
            DetailedUser user = null;

            try
            {
                var dbUser = _context.Users
                    .Include(user => user.Photos)
                    .Include(user => user.UserRoles)
                    .ThenInclude(userRole => userRole.Role)
                    .FirstOrDefault(user => user.Id == id);

                user = _mapper.Map<DetailedUser>(dbUser);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return user;
        }

        public DetailedUser GetUserDetailsByUserName(string username)
        {
            DetailedUser detailedUser = null;
            try
            {
                var user = _context.Users
                    .Include(user => user.Photos)
                    .Include(user => user.UserRoles)
                    .FirstOrDefault(u => u.UserName == username);
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

                if (searchUser.OrderBy.ToLowerInvariant() == "created")
                    query = query.OrderByDescending(u => u.Created);
                else
                    query = query.OrderByDescending(u => u.LastActive);

                var maxAge = DateTime.Now.AddYears(-searchUser.MinAge - 1);
                var minAge = DateTime.Now.AddYears(-searchUser.MaxAge - 1);

                query = query.Where(u => u.DateOfBirth >= minAge && u.DateOfBirth <= maxAge);

                if (searchUser.Gender != "all")
                    query = query.Where(q => q.Gender == searchUser.Gender);

                var userCount = query.Count();

                var itemsToSkip = (searchUser.PageNumber - 1) * searchUser.PageSize;
                query = query.Skip(itemsToSkip);
                query = query.Take(searchUser.PageSize);

                var UserList = _mapper.Map<IList<CompactUser>>(query.ToList());

                paginatedUserList = new PaginatedUserList
                {
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

        public async Task<OperationResult> CreateUser(RegisterUser registerUser)
        {
            var result = new OperationResult();
            try
            {
                var user = _mapper.Map<User>(registerUser);
                var createResult = await _userManager.CreateAsync(user, registerUser.Password);

                if (createResult.Succeeded)
                    result.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                _logger.LogError(ex.Message);
            }

            return result;
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

        public User FindUserByUserName(string username)
        {
            User user = null;
            try
            {
                user = _context.Users.FirstOrDefault(u => u.UserName == username);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return user;
        }

        public User FindUserByUserId(string userId)
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
                var user = _context.Users.Where(u => u.UserName == username).FirstOrDefault();

                if (user != null)
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
                var foundUser = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (foundUser != null)
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
                var dbUser = FindUserByUserName(updateUser.Username);

                if (dbUser == null)
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

        public bool UpdateLastActive(string userId)
        {
            bool result = false;

            try
            {
                var dbUser = FindUserByUserId(userId);

                if (dbUser == null)
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

        public async Task<LoginResult> Login(LoginUser loginUser)
        {
            LoginResult result = new LoginResult();

            try
            {
                var user = await _userManager.FindByNameAsync(loginUser.Username);

                if (user == null)
                {
                    result.Message = "User not found!";
                    return result;
                }

                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginUser.Password, false);

                if (signInResult.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var token = _authorization.GenerateToken(user, userRoles);

                    if (string.IsNullOrEmpty(token))
                    {
                        result.Message = "Token can not be generated!";
                    }
                    else
                    {
                        result.Value = token;
                        result.IsSuccessful = true;
                        result.loggedInUserId = user.Id;
                    }
                }
                else
                {
                    result.Message = "Can not sing in!";
                }

                return result;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<OperationResult> UpdateRolesAsync(string userName, RoleEdit roleEdit)
        {
            var result = new OperationResult();
            try
            {
                var user = await _userManager.FindByNameAsync(userName);

                if(user == null)
                {
                    result.Message = "User can not be found!";
                    return result;
                }

                var currentRoles = await _userManager.GetRolesAsync(user);

                var newRoles = roleEdit.Roles.Except(currentRoles);
                var deletedRoles = currentRoles.Except(roleEdit.Roles);

                await _userManager.AddToRolesAsync(user, newRoles);
                await _userManager.RemoveFromRolesAsync(user, deletedRoles);
                result.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public OperationResult<IList<UserRoleDto>> GetUserRoles()
        {
            var result = new OperationResult<IList<UserRoleDto>>();

            try
            {
                var userRoles = _context.Users
                    .Include(user => user.UserRoles)
                    .ThenInclude(role => role.Role)
                    .ToList();

                result.Value = _mapper.Map<IList<UserRoleDto>>(userRoles);
                result.IsSuccessful = true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }


    }
}