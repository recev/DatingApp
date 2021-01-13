using System.Collections.Generic;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.OperationResults;
using Microsoft.AspNetCore.Mvc;

namespace DatingApi.Data.Repositories
{
    public interface ImessageRepository
    {
        MessageDto GetDetailedMessage(int messageId);
        CreateMessageResult CreateMessage(UserMessage userMessage);
        IList<MessageDto> GetUnreadMessages(string userId);
        IList<MessageDto> GetInboxMessages(string userId);
        IList<MessageDto> GetOutboxMessages(string userId);
        IList<MessageDto> GetMessageThread(string userId, string recipientId);
        OperationResult DeleteMessage(string userId, int messageId);
        OperationResult MarkAsRead(string userId, int messageId);
    }
}