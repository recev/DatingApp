using System.Linq;
using System.Security.Claims;
using DatingApi.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace DatingApi.Controllers
{
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class PhotosController: ControllerBase
    {
        IPhotoRepository _photoRepository;

        public string CurrentUserId {
            get {
                var NameIdentifierClaim = User.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .FirstOrDefault();
                    
                return NameIdentifierClaim.Value;
            }
        }

        public PhotosController(IPhotoRepository photoRepository)
        {
            this._photoRepository = photoRepository;
        }

        [HttpDelete("{photoId}")]
        public IActionResult DeletePhoto(string userId, int photoId)
        {
            if(CurrentUserId != userId)
                return Unauthorized();

            var result = _photoRepository.DeletePhoto(CurrentUserId, photoId);

            if(result.IsSuccessful)
                return Ok();
            else
                return BadRequest(result.Message);
        }
        [HttpGet("{photoId}", Name="GetPhoto")]
        public IActionResult GetPhoto(string userId, int photoId)
        {
            var photo = _photoRepository.GetPhoto(userId, photoId);
            return Ok(photo);
        }

        [HttpPost]
        public IActionResult UploadPhoto(string userId, [FromForm] IFormFile file)
        {
            if(CurrentUserId != userId)
                return Unauthorized();

            var result = _photoRepository.UploadPhoto(userId, file);

            if(result.IsSuccessful)
                return CreatedAtRoute("GetPhoto", new {userId = CurrentUserId, photoId=result.Value.Id }, result.Value);
            else
                return BadRequest(result.Message);
        }

        [HttpPost("{photoId}/setmain")]
        public IActionResult SetMainPhoto(string userId, int photoId)
        {
            if(CurrentUserId != userId)
                return Unauthorized();

            var result = _photoRepository.SetMainPhoto(userId, photoId);

            if(result.IsSuccessful)
                return Ok();
            else
                return BadRequest(result.Message);
        }
    }
}