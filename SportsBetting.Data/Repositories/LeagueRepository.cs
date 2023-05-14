using Microsoft.EntityFrameworkCore;
using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;

namespace SportsBetting.Data.Repositories
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly SportsBettingContext _context;

        public LeagueRepository(SportsBettingContext context)
        {
            _context = context;
        }

        public async Task<List<League>>? Get()
        {
            if (_context.Leagues == null)
            {
                throw new Exception("No leagues found");
            }

            return await _context.Leagues.ToListAsync();
        }

        public async Task<League?> GetById(int Id)
        {
            return await _context.Leagues.FindAsync(Id);
        }
        public async Task<League?> GetByNameAsync(string name)
        {
            return await _context.Leagues.FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task CreateAsync(League League)
        {
            await _context.Leagues.AddAsync(League);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(League League)
        {
            _context.Leagues.Remove(League);
            await SaveChangesAsync();
        }
    }
}
