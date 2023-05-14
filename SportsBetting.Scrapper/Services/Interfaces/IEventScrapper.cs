using Kafkaproducer.DTO;

namespace KafkaProducer.Services.Interfaces
{
    public interface IEventScrapper
    {
        Task<List<EventDTO>> GetEvents();
    }
}