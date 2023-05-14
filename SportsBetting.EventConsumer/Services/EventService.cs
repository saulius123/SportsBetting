using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;
using KafkaConsumer.Services.Interfaces;

namespace KafkaConsumer.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ILeagueRepository _leagueRepository;
        private readonly ISportRepository _sportRepository;

        public EventService(IEventRepository eventRepository, ITeamRepository teamRepository, ILeagueRepository leagueRepository, ISportRepository sportRepository)
        {
            _eventRepository = eventRepository;
            _teamRepository = teamRepository;
            _leagueRepository = leagueRepository;
            _sportRepository = sportRepository;
        }
        public async Task<Event> CreateIfNotExistsAsync(EventDTO eventDTO)
        {
            int sportId = await GetOrCreateSportIdAsync(eventDTO.Sport);
            int leagueId = await GetOrCreateLeagueIdAsync(eventDTO.League, sportId);
            int team1Id = await GetOrCreateTeamIdAsync(eventDTO.Team1, leagueId);
            int team2Id = await GetOrCreateTeamIdAsync(eventDTO.Team2, leagueId);

            var existingEvent = await _eventRepository.GetByDetailsAsync(eventDTO.StartDateTime, eventDTO.EndDateTime, team1Id, team2Id, leagueId);

            if (existingEvent == null)
            {
                var newEvent = new Event
                {
                    TeamId1 = team1Id,
                    TeamId2 = team2Id,
                    StartDateTime = eventDTO.StartDateTime,
                    EndDateTime = eventDTO.EndDateTime,
                    LeagueId = leagueId,
                    IsBetsOpened = false,
                    IsResulted = false,
                };

                await _eventRepository.CreateAsync(newEvent);
                await _eventRepository.SaveChangesAsync();

                return newEvent;
            }

            return existingEvent;
        }

        private async Task<int> GetOrCreateTeamIdAsync(string teamName, int leagueId)
        {
            var team = await _teamRepository.GetByNameAsync(teamName);
            if (team == null)
            {
                team = new Team { Name = teamName, LeagueId = leagueId };
                await _teamRepository.CreateAsync(team);
                await _teamRepository.SaveChangesAsync();
            }

            return team.Id;
        }

        private async Task<int> GetOrCreateLeagueIdAsync(string leagueName, int sportId)
        {
            var league = await _leagueRepository.GetByNameAsync(leagueName);
            if (league == null)
            {
                league = new League { Name = leagueName, SportId = sportId };
                await _leagueRepository.CreateAsync(league);
                await _leagueRepository.SaveChangesAsync();
            }

            return league.Id;
        }

        private async Task<int> GetOrCreateSportIdAsync(string sportName)
        {
            var sport = await _sportRepository.GetByNameAsync(sportName);
            if (sport == null)
            {
                sport = new Sport { Name = sportName };
                await _sportRepository.CreateAsync(sport);
                await _sportRepository.SaveChangesAsync();
            }

            return sport.Id;
        }
    }
}