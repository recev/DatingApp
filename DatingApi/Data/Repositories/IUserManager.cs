using System.Collections.Generic;
using DatingApi.Data.Models;

namespace DatingApi.Data.Repositories
{
    public interface IUserManager
    {
        IList<User> GetAllUsers();
        bool DoesUserExist(string userName);
        User FindUser(string userName);
        bool SaveUser(User user);
    }
}