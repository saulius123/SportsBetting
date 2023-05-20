using SportsBetting.Data.Models;

namespace SportsBetting.Services.Services.Interfaces
{
    public interface IBetOfferService
    {
        Task<IEnumerable<BetOffer>> GetAllAsync();
        Task<BetOffer> GetByIdAsync(int id);
        Task<BetOffer> CreateAsync(BetOffer betOffer);
        Task UpdateAsync(int id, BetOffer updatedBetOffer);
        Task DeleteAsync(int id);
    }
}
