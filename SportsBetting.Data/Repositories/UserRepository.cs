using Microsoft.EntityFrameworkCore;
using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;

namespace SportsBetting.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SportsBettingContext _context;

        public UserRepository(SportsBettingContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
        }

        public async Task<User> Create(User user, string password)
        {
            if (await UserExists(user.Username))
            {
                throw new Exception("User already exists");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(user => user.Username == username);
        }
    }
}