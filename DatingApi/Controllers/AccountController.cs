using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        IAuthorization _authorization;
        IUserManager _userManager;

        public AccountController(IAuthorization authorization, IUserManager userManager)
        {
            this._authorization = authorization;
            this._userManager = userManager;
        }

        [HttpPost("Register")]
        public ActionResult Register(RegisterUser registerUser)
        {
            if(_userManager.DoesUserExist(registerUser.Username))
                return BadRequest("User already exists");

            var user = _authorization.CreateUser(registerUser.Username, registerUser.Password);

            if(user == null)
                BadRequest("User Cannot be created!");

            var isUserCreated = _userManager.SaveUser(user);

            if(isUserCreated)
                return Ok(registerUser);
            else
                return BadRequest("User can not be registered!");
        }

        [HttpPost("Login")]
        public ActionResult Login(LoginUser loginUser)
        {
            var user = _userManager.FindUser(loginUser.Username);

            if(user == null)
                return Unauthorized();

            var IsValidUser = _authorization.AreCredentialsValid(user, loginUser.Password);

            if(!IsValidUser)
                return Unauthorized();

            var token = _authorization.GenerateToken(user);

            if(string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            else
            {
                var userDetails = this._userManager.GetUserDetails(user.Id);
                return Ok(new { Token = token, User = userDetails });
            }
        } 
    }
}