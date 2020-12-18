using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

using DatingApi.Data.Repositories;


namespace DatingApi.Filters
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            var nameIdentifier = executedContext.HttpContext.User.Claims
                                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var userId = int.Parse(nameIdentifier.Value);

            var userManager = executedContext.HttpContext.RequestServices.GetService<IUserManager>();

            userManager.UpdateLastActive(userId);
        }
    }
}