using System;
using System.Collections.Generic;

namespace SportsBetting.Data.Models;

public partial class User
{
    public int Id { get; }

    public string? Name { get; set; }

    public string Email { get; set; } = null!;
}
