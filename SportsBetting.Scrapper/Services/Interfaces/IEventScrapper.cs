using SportsBetting.Scrapper.DTOs;

namespace SportsBetting.Scrapper.Services.Interfaces
{
    public interface IEventScrapper
    {
        Task<List<KafkaEventDto>> GetEvents();
    }
}