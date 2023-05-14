using System;
using System.Collections.Generic;

namespace SportsBetting.Data.Models;

public partial class League
{
    public int Id { get; }

    public string Name { get; set; } = null!;

    public int? SportId { get; set; }
}
