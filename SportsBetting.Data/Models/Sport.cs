using System;
using System.Collections.Generic;

namespace SportsBetting.Data.Models;

public partial class Sport
{
    public int Id { get; }

    public string Name { get; set; } = null!;
}
