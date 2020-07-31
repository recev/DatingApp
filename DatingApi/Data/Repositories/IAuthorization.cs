using DatingApi.Data.Models;

namespace DatingApi.Data.Repositories
{
    public interface IAuthorization
    {
        bool Register(string userName, string Password);
        User Login(string UserName, string Password);
        bool DoesUserExist(string userName);
    }
}