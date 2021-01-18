using System.Linq;
using System.Security.Claims;
using DatingApi.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApi.Controllers
{
    [Route("api/likes/{userId}")]
    [ApiController]
    public class LikesController: ControllerBase
    {
        IUserRepository _userManager;
        ILikeRepository _likeRepository;

        public string CurrentUserId {
            get {
                var NameIdentifierClaim = User.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .FirstOrDefault();
                    
                return NameIdentifierClaim.Value;
            }
        }

        public LikesController(IUserRepository userManager, ILikeRepository likeRepository)
        {
            this._userManager = userManager;
            this._likeRepository = likeRepository;
        }

        [HttpPost("{receiverId}")]
        public ActionResult LikeUser(string userId, string receiverId)
        {
             var result = _likeRepository.LikeUser(userId, receiverId);

             if(result.IsSuccessful)
                return Ok();
            else
                return BadRequest(result.Message);
        }

        [HttpGet("LikeSendedUsers")]
        public IActionResult GetLikeSendedUsers(string userId)
        {
            var users = _likeRepository.GetLikeSendedUsers(userId);
            return Ok(users);
        }

        [HttpGet("LikeReceivedFromUsers")]
        public IActionResult GetLikeReceivedFromUsers(string userId)
        {
            var users = _likeRepository.GetLikeReceivedFromUsers(userId);
            return Ok(users);
        }
    }
}