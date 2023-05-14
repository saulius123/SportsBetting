using SportsBetting.Data.Models;

namespace SportsBetting.Data.Repositories.Interfaces
{
    public interface IBetOfferRepository
    {
        Task<List<BetOffer>>? Get();
        Task CreateAsync(BetOffer BetOffer);
        Task SaveChangesAsync();
        Task<BetOffer?> GetById(int Id);
        Task RemoveAsync(BetOffer BetOffer);
    }
}
