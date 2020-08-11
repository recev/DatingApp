using DatingApi.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController: ControllerBase
    {
        IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            this._userManager = userManager;
        }

        [HttpGet("{id}")]
        public ActionResult GetUser(int id)
        {
            var user = _userManager.GetUserDetails(id);
            
            if(user == null)
                return BadRequest("user not found!");
            else
                return Ok(user);
        }

        [HttpGet("UserList")]
        public ActionResult GetUserList()
        {
            return Ok(_userManager.GetUserList());
        }

    }
}