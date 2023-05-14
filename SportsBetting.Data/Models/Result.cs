namespace SportsBetting.Data.Models;

public partial class Result
{
    public int Id { get; }

    public int EventId { get; set; }

    public int ResultTeam1 { get; set; }

    public int ResultTeam2 { get; set; }

    public int BetOfferWon { get; set; }
}
