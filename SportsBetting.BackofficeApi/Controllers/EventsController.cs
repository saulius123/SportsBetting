using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsBetting.Data.Models;
using SportsBetting.Services.Services.Interfaces;

namespace SportsBetting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: api/Events
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEvents(int pageIndex = 1, int pageSize = 20, string sortOrder = null)
        {
            var result = await _eventService.GetPagedEventsAsync(pageIndex, pageSize, sortOrder);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/Events/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return Ok(eventItem);
        }

        // POST: api/Events
        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] Event eventItem)
        {
            if (eventItem == null)
            {
                return BadRequest();
            }

            await _eventService.CreateEventAsync(eventItem);

            // Use the nameof operator to avoid hard-coding the action name in the CreatedAtAction call
            return CreatedAtAction(nameof(GetEvent), new { id = eventItem.Id }, eventItem);
        }

        // PUT: api/Events/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutEvent(int id, [FromBody] Event updatedEvent)
        {
            if (updatedEvent == null || id != updatedEvent.Id)
            {
                return BadRequest();
            }

            var eventToUpdate = await _eventService.GetEventByIdAsync(id);
            if (eventToUpdate == null)
            {
                return NotFound();
            }

            await _eventService.UpdateEventAsync(updatedEvent);
            return NoContent();
        }

        // DELETE: api/Events/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventToDelete = await _eventService.GetEventByIdAsync(id);
            if (eventToDelete == null)
            {
                return NotFound();
            }

            await _eventService.DeleteEventAsync(eventToDelete);
            return NoContent();
        }
    }
}