using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.Models;
using DatingApi.Data.OperationResults;

namespace DatingApi.Data.Repositories
{
    public interface IAuthorization
    {
        // bool AreCredentialsValid(User user, string password);
        string GenerateToken(User user, IList<string> userRoles);
    }
}