using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;

namespace SportsBetting.Data.Repositories
{
    public class ResultRepository : IResultRepository
    {
        private readonly SportsBettingContext _context;

        public ResultRepository(SportsBettingContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Result result)
        {
            await _context.Results.AddAsync(result);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Result?> GetById(int id)
        {
            return await _context.Results.FindAsync(id);
        }
    }
}