using System.Threading.Tasks;
using Dateing.API.Models;

namespace Dateing.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);

        Task<User> Login(string username, string password);

        Task<bool> UserExist(string username);
    }
}