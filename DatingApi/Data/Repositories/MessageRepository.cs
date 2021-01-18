using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using AutoMapper;

using Data;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.OperationResults;
using DatingApi.Data.Models;
using System.Collections.Generic;

namespace DatingApi.Data.Repositories
{
    public class MessageRepository : ImessageRepository
    {
        ILogger<MessageRepository> _logger;
        DatingDbContext _context;
        IMapper _mapper;
        IUserRepository _userManager;

        public MessageRepository(ILogger<MessageRepository> logger, DatingDbContext context, IMapper mapper, IUserRepository userManager)
        {
            this._logger = logger;
            this._context = context;
            this._mapper = mapper;
            this._userManager = userManager;
        }

        public OperationResult<MessageDto> CreateMessage(UserMessage userMessage)
        {
            var result = new OperationResult<MessageDto>();
            try
            {
                var senderUser = _userManager.FindUserByUserId(userMessage.SenderId);

                if(senderUser == null){
                    result.Message = "Sender can not be found!";
                    return result;
                }

                var RecipientUser = _userManager.FindUserByUserId(userMessage.RecipientId);

                if(RecipientUser == null){
                    result.Message = "Recipient user can not be found!";
                    return result;
                }

                var message = _mapper.Map<Message>(userMessage);

                result.IsSuccessful = SaveMessage(message);

                if(result.IsSuccessful)
                {
                    var savedMessage = GetDetailedMessage(message.Id);
                    result.Value = _mapper.Map<MessageDto>(savedMessage);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public bool SaveMessage(Message message)
        {
            var result = false;

            try
            {
                _context.Messages.Add(message);
                _context.SaveChanges();
                result = true;

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public MessageDto GetDetailedMessage(int messageId)
        {
            MessageDto messageDto = null;
            try
            {
                var message = _context.Messages
                                .Include(m => m.Sender)
                                .ThenInclude(m => m.Photos)
                                .Include(m => m.Recipient)
                                .ThenInclude(m => m.Photos)
                                .FirstOrDefault(m => m.Id == messageId);

                messageDto = _mapper.Map<MessageDto>(message);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return messageDto;
        }

        public IList<MessageDto> GetUnreadMessages(string userId)
        {
            IList<MessageDto> messageList = null;

            try
            {
                var messages = _context.Messages
                        .Include(m => m.Sender)
                        .ThenInclude(m => m.Photos)
                        .Include(m => m.Recipient)
                        .ThenInclude(m => m.Photos)
                        .Where(
                            m => m.IsRead == false
                            && m.RecipientId == userId
                            && m.IsRecipientDeleted == false
                        );

                messageList = _mapper.Map<IList<MessageDto>>(messages);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return messageList;
        }

        public IList<MessageDto> GetInboxMessages(string userId)
        {
            IList<MessageDto> messageList = null;

            try
            {
                var messages = _context.Messages
                        .Include(m => m.Sender)
                        .ThenInclude(m => m.Photos)
                        .Include(m => m.Recipient)
                        .ThenInclude(m => m.Photos)
                        .Where(m => 
                            m.RecipientId == userId
                            && m.IsRecipientDeleted == false
                        );

                messageList = _mapper.Map<IList<MessageDto>>(messages);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return messageList;
        }

        public IList<MessageDto> GetOutboxMessages(string userId)
        {
            IList<MessageDto> messageList = null;

            try
            {
                var messages = _context.Messages
                        .Include(m => m.Sender)
                        .ThenInclude(m => m.Photos)
                        .Include(m => m.Recipient)
                        .ThenInclude(m => m.Photos)
                        .Where(m =>
                            m.SenderId == userId
                            && m.IsSenderDeleted == false
                        );

                messageList = _mapper.Map<IList<MessageDto>>(messages);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return messageList;
        }

        public IList<MessageDto> GetMessageThread(string userId, string recipientId)
        {
            IList<MessageDto> thread = null;
            try
            {
                var messages = _context.Messages
                    .Include(m => m.Sender).ThenInclude(s => s.Photos)
                    .Include(m => m.Recipient).ThenInclude(r => r.Photos)
                    .Where(
                            m => m.SenderId == userId && m.RecipientId == recipientId && m.IsSenderDeleted == false
                            ||
                            m.SenderId == recipientId && m.RecipientId == userId && m.IsRecipientDeleted == false
                    )
                    .OrderBy(m => m.SentDate)
                    .ToList();

                thread =  _mapper.Map<IList<MessageDto>>(messages);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return thread;
        }

        public Message FindMessage(int messageId)
        {
            Message message = null;
            try
            {
                message = _context.Messages.FirstOrDefault(m => m.Id == messageId);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return message;
        }
        
        public OperationResult DeleteMessage(string userId, int messageId)
        {
            OperationResult result = new OperationResult();

            try
            {
                var message = FindMessage(messageId);

                if(message == null)
                {
                    result.Message = "Message not found!";
                    return result;
                }

                if(message.SenderId == userId)
                    message.IsSenderDeleted = true;
                
                if(message.RecipientId == userId)
                    message.IsRecipientDeleted = true;

                if(message.IsSenderDeleted == true && message.IsRecipientDeleted == true)
                    _context.Entry<Message>(message).State = EntityState.Deleted;

                _context.SaveChanges();

                result.IsSuccessful = true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }

        public OperationResult MarkAsRead(string userId, int messageId)
        {
            var result = new OperationResult();

            try
            {
                var message = FindMessage(messageId);
                message.IsRead = true;
                message.ReadDate = DateTime.Now;

                _context.SaveChanges();

                result.IsSuccessful = true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }
    }
}