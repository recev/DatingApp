using System.Collections.Generic;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.Models;

namespace DatingApi.Data.Repositories
{
    public interface IUserManager
    {   
        DetailedUser GetUserDetails(int id);
        DetailedUser GetUserDetails(string username);
        PaginatedUserList GetUserList(SearchUser searchUser);
        bool DoesUserExist(string userName);
        User FindUser(string userName);
        User FindUser(int userId);
        bool SaveUser(User user);
        bool DeleteUser(string userName);
        bool UpdateUser(UpdateUser updateUser);
        bool UpdateLastActive(int userId);
    }
}