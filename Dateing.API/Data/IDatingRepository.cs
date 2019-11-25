using System.Collections.Generic;
using System.Threading.Tasks;
using Dateing.API.Models;

namespace Dateing.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAll();

        Task<IEnumerable<User>> GetUsers();

        Task<User> GetUser(long Id);

        Task<Photo> GetPhoto(int Id);
        Task<Photo> GetMainPhotoForUser(long userId);
    }
}