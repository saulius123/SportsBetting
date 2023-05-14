using Microsoft.AspNetCore.Mvc;
using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;
using StackExchange.Redis;
using Newtonsoft.Json;
using SportsBetting.BackofficeApi.DTO;

namespace SportsBetting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        private readonly ITeamRepository _teamRepository;

        private readonly IDatabase _cache;
        public EventsController(IEventRepository eventRepository, ITeamRepository teamRepository, IConnectionMultiplexer redis)
        {
            _eventRepository = eventRepository;
            _teamRepository = teamRepository;
            _cache = redis.GetDatabase();
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult> GetEvents(int pageIndex = 1, int pageSize = 20, string? sortOrder = null)
        {
            //var result = await _eventRepository.GetPaged(pageIndex, pageSize, sortOrder);
            //return Ok(new { items = result.Items, totalItems = result.TotalItems });

            // Generate a key for caching
            string cacheKey = $"PagedEvents:page={pageIndex}:pageSize={pageSize}:sortOrder={sortOrder}";

            // Try to get the result from cache
            var cachedResult = _cache.StringGet(cacheKey);
            if (cachedResult.HasValue)
            {
                // If the result is in the cache, deserialize it and return
                var result = JsonConvert.DeserializeObject<PagedResultDto<Event>>(cachedResult);
                return Ok(new { items = result.Items, totalItems = result.TotalItems });
            }
            else
            {
                // If the result is not in the cache, get it from the repository
                var result = await _eventRepository.GetPaged(pageIndex, pageSize, sortOrder);

                // Store the result in the cache for future requests
                _cache.StringSet(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(5));

                return Ok(new { items = result.Items, totalItems = result.TotalItems });
            }
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            Event? Event =  await _eventRepository.GetById(id);
            if (Event == null)
            {
                return NotFound();
            }

            return Event;

        }

        // POST: api/Events
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event Event)
        {
            //get all keys that start with the "PagedEvents" pattern
            var server = _cache.Multiplexer.GetServer(_cache.Multiplexer.GetEndPoints().First());
            var keys = server.Keys(pattern: "PagedEvents*");

            //delete each key
            foreach (var key in keys)
            {
                _cache.KeyDelete(key);
            }

            await _eventRepository.CreateAsync(Event);
            await _eventRepository.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = Event.Id }, Event);
        }

        // PUT: api/Events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event updatedEvent)
        {
            if (id != updatedEvent.Id)
            {
                return BadRequest();
            }

            var team1 = await _teamRepository.GetById(updatedEvent.TeamId1);
            if (team1 == null)
            {
                return BadRequest($"Team with ID {updatedEvent.TeamId1} does not exist.");
            }

            var team2 = await _teamRepository.GetById(updatedEvent.TeamId2);
            if (team2 == null)
            {
                return BadRequest($"Team with ID {updatedEvent.TeamId2} does not exist.");
            }

            var existingEvent = await _eventRepository.GetById(id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            existingEvent.StartDateTime = updatedEvent.StartDateTime;
            existingEvent.EndDateTime = updatedEvent.EndDateTime;
            existingEvent.LeagueId = updatedEvent.LeagueId;
            existingEvent.TeamId1 = updatedEvent.TeamId1;
            existingEvent.TeamId2 = updatedEvent.TeamId2;
            existingEvent.IsBetsOpened = updatedEvent.IsBetsOpened;
            existingEvent.IsResulted = updatedEvent.IsResulted;

            await _eventRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var Event = await _eventRepository.GetById(id);
            if (Event == null)
            {
                return NotFound();
            }

            await _eventRepository.RemoveAsync(Event);
            await _eventRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
