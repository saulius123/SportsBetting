using Microsoft.EntityFrameworkCore;
using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;

namespace SportsBetting.Data.Repositories
{
    public class SportRepository : ISportRepository
    {
        private readonly SportsBettingContext _context;

        public SportRepository(SportsBettingContext context)
        {
            _context = context;
        }

        public async Task<List<Sport>>? Get()
        {
            if (_context.Sports == null)
            {
                throw new Exception("No sport found");
            }

            return await _context.Sports.ToListAsync();
        }

        public async Task<Sport?> GetById(int Id)
        {
            return await _context.Sports.FindAsync(Id);
        }
        public async Task<Sport?> GetByNameAsync(string name)
        {
            return await _context.Sports.FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task CreateAsync(Sport Sport)
        {
            await _context.Sports.AddAsync(Sport);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Sport Sport)
        {
            _context.Sports.Remove(Sport);
            await SaveChangesAsync();
        }
    }
}
