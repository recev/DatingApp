using System;
using System.Collections.Generic;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.OperationResults;

namespace DatingApi.Data.Repositories
{
    public interface ILikeRepository
    {
        OperationResult LikeUser(string userId, string receiverId);
        IList<CompactUser> GetLikeSendedUsers(string userId);
        IList<CompactUser> GetLikeReceivedFromUsers(string userId);
    }
}