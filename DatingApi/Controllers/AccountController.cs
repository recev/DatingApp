using System.Threading.Tasks;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.OperationResults;
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
        IUserRepository _userRepository;

        public AccountController(IAuthorization authorization, IUserRepository userRepository)
        {
            this._authorization = authorization;
            this._userRepository = userRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUser registerUser)
        {
            if(_userRepository.DoesUserExist(registerUser.Username))
                return BadRequest("User already exists");

            var result = await _userRepository.CreateUser(registerUser);

            if(result.IsSuccessful)
            {
                var detailedUser = _userRepository.GetUserDetailsByUserName(registerUser.Username);
                return CreatedAtRoute("GetUser", new { Controller= "Users", id = detailedUser.Id }, detailedUser);
            }
            else
                return BadRequest("User can not be registered!");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            var loginResult = await _userRepository.Login(loginUser);

            if (loginResult.IsSuccessful)
            {
                var userDetails = this._userRepository.GetUserDetailsByUserId(loginResult.loggedInUserId);
                return Ok(new { Token = loginResult.Value, User = userDetails });
            }
            else
            {
                return BadRequest(loginResult.Message);
            }
        }
    }
}