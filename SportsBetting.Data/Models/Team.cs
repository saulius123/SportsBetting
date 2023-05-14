using System.ComponentModel.DataAnnotations.Schema;

namespace SportsBetting.Data.Models;

[Table("Teams")]
public partial class Team
{
    public int Id { get; }

    public int LeagueId { get; set; }

    public string Name { get; set; } = null!;
}
