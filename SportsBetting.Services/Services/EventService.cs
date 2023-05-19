using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;
using SportsBetting.Services.Services.Interfaces;
using SportsBetting.Services.DTOs;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly ITeamRepository _teamRepository;

    public EventService(IEventRepository eventRepository, ITeamRepository teamRepository)
    {
        _eventRepository = eventRepository;
        _teamRepository = teamRepository;
    }

    public async Task<PagedResultDto<Event>> GetPagedEventsAsync(int pageIndex, int pageSize, string sortOrder)
    {
        var (items, totalItems) = await _eventRepository.GetPaged(pageIndex, pageSize, sortOrder);
        return new PagedResultDto<Event>
        {
            Items = items.ToList(),
            TotalItems = totalItems
        };
    }

    public async Task<Event> GetEventByIdAsync(int id)
    {
        return await _eventRepository.GetById(id);
    }

    public async Task CreateEventAsync(Event eventItem)
    {
        await _eventRepository.CreateAsync(eventItem);
        await _eventRepository.SaveChangesAsync();
    }

    public async Task UpdateEventAsync(Event eventToUpdate)
    {
        var team1 = await _teamRepository.GetById(eventToUpdate.TeamId1);
        if (team1 == null)
        {
            throw new Exception($"Team with ID {eventToUpdate.TeamId1} does not exist.");
        }

        var team2 = await _teamRepository.GetById(eventToUpdate.TeamId2);
        if (team2 == null)
        {
            throw new Exception($"Team with ID {eventToUpdate.TeamId2} does not exist.");
        }

        var existingEvent = await _eventRepository.GetById(eventToUpdate.Id);
        if (existingEvent == null)
        {
            throw new Exception("Event not found.");
        }

        existingEvent.StartDateTime = eventToUpdate.StartDateTime;
        existingEvent.EndDateTime = eventToUpdate.EndDateTime;
        existingEvent.LeagueId = eventToUpdate.LeagueId;
        existingEvent.TeamId1 = eventToUpdate.TeamId1;
        existingEvent.TeamId2 = eventToUpdate.TeamId2;
        existingEvent.IsBetsOpened = eventToUpdate.IsBetsOpened;
        existingEvent.IsResulted = eventToUpdate.IsResulted;

        await _eventRepository.SaveChangesAsync();
    }

    public async Task DeleteEventAsync(Event eventToDelete)
    {
        await _eventRepository.RemoveAsync(eventToDelete);
        await _eventRepository.SaveChangesAsync();
    }
}
