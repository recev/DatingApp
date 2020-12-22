using System.IO;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class FallbackController: ControllerBase
    {
        public IActionResult Index()
        {
            var indexFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html");
            return PhysicalFile(indexFilePath, "text/html");
        }
    }
}