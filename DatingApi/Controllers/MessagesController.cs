using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;
using System.Linq;
using System.Security.Claims;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.OperationResults;
using DatingApi.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users/{userID}/[controller]")]
    public class MessagesController: ControllerBase
    {
        ImessageRepository _messageRepository;


        public int CurrentUserId {
            get {
                var NameIdentifierClaim = User.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .FirstOrDefault();
                    
                return int.Parse(NameIdentifierClaim.Value);
            }
        }

        public MessagesController(ImessageRepository messageRepository)
        {
            this._messageRepository = messageRepository;
        }

        [HttpGet("{messageId}", Name = "GetMessage")]
        public IActionResult GetMessage(int userId, int messageId)
        {
            if(CurrentUserId != userId)
                return Unauthorized();

            var message = this._messageRepository.GetDetailedMessage(messageId);
            
            if(message == null)
                return BadRequest("Message not found!");
            else
                return Ok(message);
        }

        [HttpPost]
        public IActionResult CreateMessage(int userId, UserMessage userMessage)
        {
            if(CurrentUserId != userId)
                return Unauthorized();

            userMessage.SenderId = userId;
            var result = _messageRepository.CreateMessage(userMessage);

            if(result.IsSuccessful)
                return CreatedAtRoute("GetMessage", new { UserId = userId, MessageId = result.CreatedMessage.Id }, result.CreatedMessage);
            else
                return BadRequest(result.CreatedMessage);
        }

        [HttpGet("unread")]
        public IActionResult GetUnreadMessages(int userId)
        {
            var messages = _messageRepository.GetUnreadMessages(userId);

            if(messages == null)
                return BadRequest("No unread messages!");
            else
                return Ok(messages);
        }

        [HttpGet("inbox")]
        public IActionResult GetInboxMessages(int userId)
        {
            var messages = _messageRepository.GetInboxMessages(userId);

            if(messages == null)
                return BadRequest("No inbox messages!");
            else
                return Ok(messages);
        }

        [HttpGet("outbox")]
        public IActionResult GetOutboxMessages(int userId)
        {
            var messages = _messageRepository.GetOutboxMessages(userId);

            if(messages == null)
                return BadRequest("No outbox messages!");
            else
                return Ok(messages);
        }

        [HttpGet("thread/{recipientId}")]
        public IActionResult GetMessageThread(int userId, int recipientId)
        {
            var messageThread = _messageRepository.GetMessageThread(userId, recipientId);

            if(messageThread == null)
                return BadRequest("Messages not found!");
            else
                return Ok(messageThread);
        }

        [HttpDelete("{messageId}")]
        public IActionResult DeleteMessage(int userId, int messageId)
        {
            if(CurrentUserId != userId)
                return Unauthorized();

            var result = _messageRepository.DeleteMessage(userId, messageId);

            if(result.IsSuccessful)
                return NoContent();
            else
                return BadRequest(result.Message);
        }

        [HttpPost("{messageId}/read")]
        public IActionResult MarkAsRead(int userId, int messageId)
        {
            if(CurrentUserId != userId)
                return Unauthorized();

            var result = _messageRepository.MarkAsRead(userId, messageId);

            if(result.IsSuccessful)
                return Ok();
            else
                return BadRequest(result.Message);
        }
    }
}