using Microsoft.EntityFrameworkCore;
using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;

namespace SportsBetting.Data.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly SportsBettingContext _context;

        public EventRepository(SportsBettingContext context)
        {
            _context = context;
        }

        public async Task<List<Event>>? Get()
        {
            if (_context.Events == null)
            {
                throw new Exception("No BetOffers found");
            }

            return await _context.Events.ToListAsync();
        }

        public async Task<(IList<Event> Items, int TotalItems)> GetPaged(int pageIndex, int pageSize, string? sortOrder = null)
        {
            IQueryable<Event> query = _context.Events;

            if (!string.IsNullOrWhiteSpace(sortOrder))
            {
                switch (sortOrder)
                {
                    case "start_date_desc":
                        query = query.OrderByDescending(s => s.StartDateTime);
                        break;
                    default:
                        query = query.OrderBy(s => s.StartDateTime);
                        break;
                }
            }

            int totalItems = await query.CountAsync();

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return (Items: await query.ToListAsync(), TotalItems: totalItems);
        }

        public async Task<Event?> GetById(int Id)
        {
            return await _context.Events.FindAsync(Id);
        }

        public async Task CreateAsync(Event Event)
        {
            await _context.Events.AddAsync(Event);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Event Event)
        {
            _context.Events.Remove(Event);
            await SaveChangesAsync();
        }

        public async Task<Event?> GetByDetailsAsync(DateTime startTime, DateTime endTime, int team1Id, int team2Id, int leagueId)
        {
            return await _context.Events
                .Where(e => e.StartDateTime == startTime &&
                            e.EndDateTime == endTime &&
                            e.TeamId1 == team1Id &&
                            e.TeamId2 == team2Id &&
                            e.LeagueId == leagueId)
                .FirstOrDefaultAsync();
        }
    }
}
