using System.Threading.Tasks;
using SportsBetting.Data.Models;

namespace KafkaConsumer.Services.Interfaces
{
    public interface IEventService
    {
        Task<Event> CreateIfNotExistsAsync(EventDTO eventDTO);
    }
}