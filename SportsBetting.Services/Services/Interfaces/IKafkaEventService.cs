using SportsBetting.Data.Models;
using SportsBetting.Services.DTOs;

namespace SportsBetting.Services.Services.Interfaces
{
    public interface IKafkaEventService
    {
        Task<Event> CreateIfNotExistsAsync(KafkaEventDto eventDto);
    }
}