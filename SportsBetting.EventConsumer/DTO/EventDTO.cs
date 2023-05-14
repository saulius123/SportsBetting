using System.ComponentModel.DataAnnotations;

public class EventDTO
{
    [Required]
    public string Team1 { get; set; }

    [Required]
    public string Team2 { get; set; }

    [Required]
    public DateTime StartDateTime { get; set; }

    [Required]
    public DateTime EndDateTime { get; set; }

    [Required]
    public string League { get; set; }

    [Required]
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