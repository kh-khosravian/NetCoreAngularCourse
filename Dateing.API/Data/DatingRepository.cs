using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dateing.API.Helpers;
using Dateing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Dateing.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(long userId, long recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l => l.LikerId == userId && l.LikeeId == recipientId);
        }

        public async Task<Photo> GetMainPhotoForUser(long userId)
        {
            return await _context.Photos.FirstOrDefaultAsync(x => x.IsMain && x.UserId == userId);
        }

        public async Task<Photo> GetPhoto(int Id)
        {
            return await _context.Photos.FindAsync(Id);
        }

        public async Task<User> GetUser(long id)
        {
            var user = await _context.Users.Include("Photos").FirstOrDefaultAsync(f => f.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);
            users = users.Where(u => u.Gender == userParams.Gender);

            if (userParams.Likers)
            {
                var userLikers = await GetUserLike(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }
            if (userParams.Likees)
            {
                var userLikees = await GetUserLike(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDbo = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.Where(u => u.BirthDay >= minDob && u.BirthDay <= maxDbo);
            }
            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }
        private async Task<IEnumerable<long>> GetUserLike(long id, bool likers)
        {
            var user = await _context.Users
            .Include(u => u.Likees)
            .Include(u => u.Likers).FirstOrDefaultAsync(user => user.Id == id);
            if (likers)
            {
                return user.Likers.Select(s => s.LikerId).ToList();
            }
            else
            {
                return user.Likees.Select(s => s.LikeeId).ToList();
            }
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetMessage(long id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessagesParam messagesParam)
        {
            var messages = _context.Messages.Include(u => u.Sender)
            .ThenInclude(p => p.Photos)
            .Include(u => u.Recipient).ThenInclude(p => p.Photos).AsQueryable();
            switch (messagesParam.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(m => m.RecipientId == messagesParam.UserId);
                    break;
                case "Outbox":
                    messages = messages.Where(m => m.SenderId == messagesParam.UserId);
                    break;
                default:
                    messages = messages.Where(m => m.RecipientId == messagesParam.UserId && !m.IsRead);
                    break;
            }
            messages = messages.OrderByDescending(d => d.MessageSent);
            return await PagedList<Message>.CreateAsync(messages, messagesParam.PageNumber, messagesParam.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(long userId, long recipientId)
        {
            var messages = await _context.Messages.Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos).AsQueryable()
                .Where(m => m.RecipientId == userId & m.SenderId == recipientId
                    || m.RecipientId == userId && m.SenderId == recipientId).OrderByDescending(m => m.MessageSent).ToListAsync();
            return messages;
        }
    }
}