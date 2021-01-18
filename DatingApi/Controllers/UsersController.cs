using System.Security.Claims;
using System.Linq;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatingApi.Filters;
using System.Threading.Tasks;
namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    public class UsersController: ControllerBase
    {
        IUserRepository _userRepository;

        public string CurrentUserId {
            get {
                var NameIdentifierClaim = User.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .FirstOrDefault();
                    
                return NameIdentifierClaim.Value;
            }
        }

        public UsersController(IUserRepository userManager)
        {
            this._userRepository = userManager;
        }

        [HttpGet("{id}", Name="GetUser")]
        // [Authorize(Roles = "Member")]
        [Authorize(Policy = "MemberPolicy")]
        public ActionResult GetUser(string id)
        {
            var user = _userRepository.GetUserDetailsByUserId(id);
            
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

            var result = _userRepository.UpdateUser(updateUser);

            if(result)
                return Ok(updateUser);
            else
                return BadRequest("User can not be updated!");
        }

        [HttpGet("UserList")]
        // [Authorize(Roles = "Admin")]
        [Authorize(Policy = "ModeratorPolicy")]
        public ActionResult GetUserList([FromQuery] SearchUser searchUser)
        {
            return Ok(_userRepository.GetUserList(searchUser));
        }

        [HttpGet("UserRoles")]
        [Authorize(Policy = "ModeratorPolicy")]
        public IActionResult GetUserRoles()
        {
            var result = _userRepository.GetUserRoles();

            if(result.IsSuccessful)
                return Ok(result.Value);
            else
                return BadRequest(result.Message);
        }


        [Authorize(Policy = "ModeratorPolicy")]
        [HttpPost("editRole/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEdit roleEdit)
        {
            var result = await _userRepository.UpdateRolesAsync(userName, roleEdit);

            if(result.IsSuccessful)
                return Ok();
            else
                return BadRequest(result.Message);
        }
    }
}