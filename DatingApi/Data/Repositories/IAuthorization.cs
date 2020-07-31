using DatingApi.Data.Models;

namespace DatingApi.Data.Repositories
{
    public interface IAuthorization
    {
        bool AreCredentialsValid(User user, string password);
        string GenerateToken(User user);
        User CreateUser(string userName, string Password);
    }
}