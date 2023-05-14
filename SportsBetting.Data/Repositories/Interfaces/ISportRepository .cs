using SportsBetting.Data.Models;

namespace SportsBetting.Data.Repositories.Interfaces
{
    public interface ISportRepository
    {
        Task<List<Sport>>? Get();
        Task CreateAsync(Sport Sport);
        Task SaveChangesAsync();
        Task<Sport?> GetById(int Id);
        Task RemoveAsync(Sport Sport);
        Task<Sport?> GetByNameAsync(string name);
    }
}