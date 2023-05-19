using SportsBetting.Services.DTOs;
using SportsBetting.Data.Models;

namespace SportsBetting.Services.Services.Interfaces
{
    interface IEventService
    {
        Task<PagedResultDto<Event>> GetPagedEventsAsync(int pageIndex, int pageSize, string sortOrder);
        Task<Event> GetEventByIdAsync(int id);
        Task CreateEventAsync(Event eventItem);
        Task UpdateEventAsync(Event eventToUpdate);
        Task DeleteEventAsync(Event eventToDelete);
    }
}
