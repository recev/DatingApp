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
        IList<MessageDto> GetUnreadMessages(int userId);
        IList<MessageDto> GetInboxMessages(int userId);
        IList<MessageDto> GetOutboxMessages(int userId);
        IList<MessageDto> GetMessageThread(int userId, int recipientId);
        OperationResult DeleteMessage(int userId, int messageId);
        OperationResult MarkAsRead(int userId, int messageId);
    }
}