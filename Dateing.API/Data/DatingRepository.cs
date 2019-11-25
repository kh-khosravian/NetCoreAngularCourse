using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include("Photos").ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}