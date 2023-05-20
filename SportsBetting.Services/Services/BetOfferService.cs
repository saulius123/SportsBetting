using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;

namespace SportsBetting.Services.Services
{
    internal class BetOfferService
    {
        private readonly IBetOfferRepository _betOfferRepository;

        public BetOfferService(IBetOfferRepository betOfferRepository)
        {
            _betOfferRepository = betOfferRepository;
        }

        public async Task<IEnumerable<BetOffer>> GetAllAsync()
        {
            return await _betOfferRepository.Get();
        }

        public async Task<BetOffer> GetByIdAsync(int id)
        {
            return await _betOfferRepository.GetById(id);
        }

        public async Task<BetOffer> CreateAsync(BetOffer betOffer)
        {
            await _betOfferRepository.CreateAsync(betOffer);
            await _betOfferRepository.SaveChangesAsync();
            return betOffer;
        }

        public async Task UpdateAsync(int id, BetOffer updatedBetOffer)
        {
            var existingBetOffer = await _betOfferRepository.GetById(id);

            if (existingBetOffer != null)
            {
                existingBetOffer.TypeId = updatedBetOffer.TypeId;
                existingBetOffer.Odd = updatedBetOffer.Odd;
                await _betOfferRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var betOffer = await _betOfferRepository.GetById(id);
            if (betOffer != null)
            {
                await _betOfferRepository.RemoveAsync(betOffer);
                await _betOfferRepository.SaveChangesAsync();
            }
        }
    }
}
