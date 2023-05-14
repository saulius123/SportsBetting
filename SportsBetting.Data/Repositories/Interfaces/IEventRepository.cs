using SportsBetting.Data.Models;

namespace SportsBetting.Data.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<List<Event>>? Get();
        Task<(IList<Event> Items, int TotalItems)> GetPaged(int pageIndex, int pageSize, string? sortOrder);
        Task CreateAsync(Event Event);
        Task SaveChangesAsync();
        Task<Event?> GetById(int Id);
        Task RemoveAsync(Event Event);
        public Task<Event?> GetByDetailsAsync(DateTime startTime, DateTime endTime, int team1Id, int team2Id, int leagueId);
    }
}