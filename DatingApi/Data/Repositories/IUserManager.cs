using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.Models;
using DatingApi.Data.OperationResults;

namespace DatingApi.Data.Repositories
{
    public interface IuserRepository
    {   
        DetailedUser GetUserDetailsByUserId(string id);
        DetailedUser GetUserDetailsByUserName(string username);
        PaginatedUserList GetUserList(SearchUser searchUser);
        bool DoesUserExist(string userName);
        User FindUserByUserName(string userName);
        User FindUserByUserId(string userId);
        Task<OperationResult> CreateUser(RegisterUser registerUser);
        bool SaveUser(User user);
        bool DeleteUser(string userName);
        bool UpdateUser(UpdateUser updateUser);
        bool UpdateLastActive(string userId);
        Task<LoginResult> Login(LoginUser loginUser);
        Task<OperationResult> UpdateRolesAsync(string userName, RoleEdit roleEdit);
    }
}