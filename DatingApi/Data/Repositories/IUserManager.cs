using System.Collections.Generic;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.Models;

namespace DatingApi.Data.Repositories
{
    public interface IUserManager
    {   
        DetailedUser GetUserDetails(int id);
        IList<CompactUser> GetUserList();
        bool DoesUserExist(string userName);
        User FindUser(string userName);
        bool SaveUser(User user);
        bool DeleteUser(string userName);
    }
}