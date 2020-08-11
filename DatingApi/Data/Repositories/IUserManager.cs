using System.Collections.Generic;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.Models;

namespace DatingApi.Data.Repositories
{
    public interface IUserManager
    {   
        UserForDetail GetUserDetails(int id);
        IList<UserForList> GetUserList();
        bool DoesUserExist(string userName);
        User FindUser(string userName);
        bool SaveUser(User user);
        bool DeleteUser(string userName);
    }
}