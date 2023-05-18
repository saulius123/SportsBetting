using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;
using SportsBetting.Services.Services.Interfaces;

namespace SportsBetting.Services.Services
{
    public class ResultService : IResultService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IResultRepository _resultRepository;

        public ResultService(IEventRepository eventRepository, IResultRepository resultRepository)
        {
            _eventRepository = eventRepository;
            _resultRepository = resultRepository;
        }

        public async Task CreateResultAsync(Result result)
        {
            var eventObj = await _eventRepository.GetById(result.EventId);

            if (eventObj == null)
            {
                throw new Exception($"Event with ID {result.EventId} does not exist.");
            }

            if (eventObj.IsResulted == true)
            {
                throw new Exception($"Event with ID {result.EventId} is already resulted.");
            }

            eventObj.IsResulted = true;
            await _eventRepository.SaveChangesAsync();

            await _resultRepository.CreateAsync(result);
            await _resultRepository.SaveChangesAsync();
        }

        public async Task<Result?> GetResultByIdAsync(int id)
        {
            return await _resultRepository.GetById(id);
        }
    }
}