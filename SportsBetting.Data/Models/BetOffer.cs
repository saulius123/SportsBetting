namespace SportsBetting.Data.Models;

public partial class BetOffer
{
    public int Id { get; }

    public int? EventId { get; set; }

    public int? TypeId { get; set; }

    public int? Odd { get; set; }
}
