using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [AllowAnonymous]
    public class FallbackController: ControllerBase
    {
        public IActionResult Index()
        {
            var indexFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html");
            return PhysicalFile(indexFilePath, "text/html");
        }
    }
}