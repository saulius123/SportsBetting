using Microsoft.EntityFrameworkCore;
using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;

namespace SportsBetting.Data.Repositories
{
    public class BetOfferRepository : IBetOfferRepository
    {
        private readonly SportsBettingContext _context;

        public BetOfferRepository(SportsBettingContext context)
        {
            _context = context;
        }

        public async Task<List<BetOffer>>? Get()
        {
            if (_context.BetOffers == null)
            {
                throw new Exception("No BetOffers found");
            }

            /*switch (sortOrder)
            {
                case "start_date":
                //return await _context.BetOffers.OrderBy(s => s.StartDateTime).ToListAsync();
                case "start_date_desc":
                //return await _context.BetOffers.OrderByDescending(s => s.StartDateTime).ToList();
                default:
                    break;
            }*/


            return await _context.BetOffers.ToListAsync();
        }

        public async Task<BetOffer?> GetById(int Id)
        {
            return await _context.BetOffers.FindAsync(Id);
        }

        public async Task CreateAsync(BetOffer BetOffer)
        {
            await _context.BetOffers.AddAsync(BetOffer);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(BetOffer BetOffer)
        {
            _context.BetOffers.Remove(BetOffer);
            await SaveChangesAsync();
        }
    }
}
