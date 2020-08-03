using System;
using DatingApi.Data.DataTransferObjects;
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
        IAuthorization _authorization;
        IUserManager _userManager;

        public UsersController(IAuthorization authorization, IUserManager userManager)
        {
            this._authorization = authorization;
            this._userManager = userManager;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public ActionResult Register(RegisterUser registerUser)
        {
            if(_userManager.DoesUserExist(registerUser.UserName))
                return BadRequest("User already exists");

            var user = _authorization.CreateUser(registerUser.UserName, registerUser.Password);

            if(user == null)
                BadRequest("User Cannot be created!");

            var isUserCreated = _userManager.SaveUser(user);

            if(isUserCreated)
                return Ok(registerUser);
            else
                return BadRequest("User can not be registered!");
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public ActionResult Login(LoginUser loginUser)
        {
            var user = _userManager.FindUser(loginUser.UserName);

            if(user == null)
                return Unauthorized();

            var IsValidUser = _authorization.AreCredentialsValid(user, loginUser.Password);

            if(!IsValidUser)
                return Unauthorized();

            var token = _authorization.GenerateToken(user);

            if(string.IsNullOrEmpty(token))
                return Unauthorized();
            else
                return Ok(new { Token = token });
        } 
        
        [AllowAnonymous]
        [HttpGet("AllUsers")]
        public ActionResult GetUsers()
        {
            return Ok(_userManager.GetAllUsers());
        }

    }
}