using Microsoft.EntityFrameworkCore;
using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;

namespace SportsBetting.Data.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly SportsBettingContext _context;

        public TeamRepository(SportsBettingContext context)
        {
            _context = context;
        }

        public async Task<List<Team>>? Get()
        {
            if (_context.Teams == null)
            {
                throw new Exception("No teams found");
            }

            return await _context.Teams.ToListAsync();
        }

        public async Task<Team?> GetById(int Id)
        {
            return await _context.Teams.FindAsync(Id);
        }

        public async Task CreateAsync(Team Team)
        {
            await _context.Teams.AddAsync(Team);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Team Team)
        {
            _context.Teams.Remove(Team);
            await SaveChangesAsync();
        }
        public async Task<Team?> GetByNameAsync(string name)
        {
            return await _context.Teams.FirstOrDefaultAsync(t => t.Name == name);
        }
    }
}
