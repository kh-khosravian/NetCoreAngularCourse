using System.Collections.Generic;
using System.Threading.Tasks;
using Dateing.API.Helpers;
using Dateing.API.Models;

namespace Dateing.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAll();

        Task<PagedList<User>> GetUsers(UserParams userParams);

        Task<User> GetUser(long Id);

        Task<Photo> GetPhoto(int Id);
        Task<Photo> GetMainPhotoForUser(long userId);

        Task<Like> GetLike(long userId, long recipientId);

        Task<Message> GetMessage(long id);
        Task<PagedList<Message>> GetMessagesForUser(MessagesParam messagesParam);

        Task<IEnumerable<Message>> GetMessageThread(long userId, long recipientId);
    }
}