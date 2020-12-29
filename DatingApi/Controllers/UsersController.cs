using System.Security.Claims;
using System.Linq;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatingApi.Filters;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(LogUserActivity))]
    public class UsersController: ControllerBase
    {
        IUserManager _userManager;

        public int CurrentUserId {
            get {
                var NameIdentifierClaim = User.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .FirstOrDefault();
                    
                return int.Parse(NameIdentifierClaim.Value);
            }
        }

        public UsersController(IUserManager userManager)
        {
            this._userManager = userManager;
        }

        [HttpGet("{id}", Name="GetUser")]
        public ActionResult GetUser(int id)
        {
            var user = _userManager.GetUserDetails(id);
            
            if(user == null)
                return BadRequest("user not found!");
            else
                return Ok(user);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUser(UpdateUser updateUser)
        {
            if(CurrentUserId != updateUser.Id)
                return Unauthorized();

            var result = _userManager.UpdateUser(updateUser);

            if(result)
                return Ok(updateUser);
            else
                return BadRequest("User can not be updated!");
        }

        [HttpGet("UserList")]
        public ActionResult GetUserList([FromQuery] SearchUser searchUser)
        {
            return Ok(_userManager.GetUserList(searchUser));
        }
    }
}