namespace SportsBetting.Data.Models;

public class Event
{
    public int Id { get; }
    public int TeamId1 { get; set; }
    public int TeamId2 { get; set; }
    public string? TeamName { get; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public int LeagueId { get; set; }
    public bool? IsBetsOpened { get; set; }
    public bool? IsResulted { get; set; }
}
