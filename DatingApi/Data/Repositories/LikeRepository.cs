using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Data;
using DatingApi.Data.DataTransferObjects;
using DatingApi.Data.Models;
using DatingApi.Data.OperationResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DatingApi.Data.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        ILogger<LikeRepository> _logger;
        DatingDbContext _context;
        IuserRepository _userManager;
        IMapper _mapper;

        public LikeRepository(DatingDbContext context, ILogger<LikeRepository> logger, IuserRepository userManager, IMapper mapper)
        {
            this._context = context;
            this._logger = logger;
            this._userManager = userManager;
            this._mapper = mapper;
        }

        public IList<CompactUser> GetLikeSendedUsers(string userId)
        {
            IList<CompactUser> users = null;
            try
            {
                var likedSendedUsers = _context.Likes
                    .Include(l => l.Receiver)
                    .Include(l => l.Receiver.Photos)
                    .Where(l => l.SenderId == userId)
                    .Select(l => l.Receiver)
                    .ToList();
                users = this._mapper.Map<IList<CompactUser>>(likedSendedUsers);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return users;
        }

        public IList<CompactUser> GetLikeReceivedFromUsers(string userId)
        {
            IList<CompactUser> users = null;
            try
            {
                var likeReceivedFromUsers = _context.Likes
                .Include(l => l.Sender)
                .Include(l => l.Sender.Photos)
                .Where(l => l.ReceivedId == userId)
                .Select(l => l.Sender)
                .ToList();
                users = this._mapper.Map<IList<CompactUser>>(likeReceivedFromUsers);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return users;
        }

        public OperationResult LikeUser(string userId, string receiverId)
        {
            var result = new OperationResult();

            var like = GetLike(userId, receiverId);

            if(like != null)
            {
                result.Message = "User is already liked!";
                return result;
            }

            var likeReceiver = _userManager.FindUserByUserId(receiverId);

            if (likeReceiver == null)
            {
                result.Message = "Liked user can't be found!";
                return result;
            }

            var isSaved = SaveLike(userId, receiverId);

            if(isSaved)
            {
                result.Message = "User liked successfully!";
                result.IsSuccessful = true;
            }
            else
            {
                result.Message = "User cannot be liked!";
            }

            return result;
        }

        public Like GetLike(string userId, string receiverId)
        {
            Like like = null;
            try
            {
                like = _context.Likes
                .Where(l => l.SenderId == userId && l.ReceivedId == receiverId)
                .FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return like;
        }

        public bool SaveLike(string userId, string receiverId)
        {
            var result = false;

            try
            {
                var newLike = new Like{
                    SenderId = userId,
                    ReceivedId = receiverId
                };

                _context.Likes.Add(newLike);
                _context.SaveChanges();

                result = true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }

    }
}