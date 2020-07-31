using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: ControllerBase
    {
        IAuthorization _authorization;
        public UsersController(IAuthorization authorization)
        {
            this._authorization = authorization;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok("aget");
        }

        [HttpPost("Register")]
        public ActionResult Register(RegisterUser registerUser)
        {
            if(_authorization.DoesUserExist(registerUser.UserName))
                return BadRequest("User already exists");
            
            var isUserCreated = _authorization.Register(registerUser.UserName, registerUser.Password);

            if(isUserCreated)
                return Ok(registerUser);
            else
                return BadRequest("User can not be registered!");
        }

    }
}