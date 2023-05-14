namespace Kafkaproducer.DTO;

public class EventDTO
{
    public string Team1 { get; set; }
    public string Team2 { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string League { get; set; }

    public string Sport { get; set; }

    public EventDTO(string team1, string team2, DateTime startDateTime, DateTime endDateTime, string league, string sport)
    {
        Team1 = team1;
        Team2 = team2;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        League = league;
        Sport = sport;
    }
}
