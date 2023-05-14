using SportsBetting.Data.Models;

namespace SportsBetting.Data.Repositories.Interfaces
{
    public interface ILeagueRepository
    {
        Task<List<League>>? Get();
        Task CreateAsync(League League);
        Task SaveChangesAsync();
        Task<League?> GetById(int Id);
        Task RemoveAsync(League League);
        Task<League?> GetByNameAsync(string name);
    }
}