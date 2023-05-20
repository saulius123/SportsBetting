using SportsBetting.Data.Models;

namespace SportsBetting.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(string username);
        Task<User> Create(User user, string password);
        Task<bool> UserExists(string username);
    }
}