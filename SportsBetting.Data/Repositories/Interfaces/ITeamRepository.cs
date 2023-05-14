using SportsBetting.Data.Models;

namespace SportsBetting.Data.Repositories.Interfaces
{
    public interface ITeamRepository
    {
        Task<List<Team>>? Get();
        Task CreateAsync(Team Team);
        Task SaveChangesAsync();
        Task<Team?> GetById(int Id);
        Task RemoveAsync(Team Team);
        Task<Team?> GetByNameAsync(string name);
    }
}