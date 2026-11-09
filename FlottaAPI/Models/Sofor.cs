using System;
using System.Collections.Generic;

namespace FlottaAPI.Models;

public partial class Sofor
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string Image { get; set; } = null!;

    public virtual ICollection<Kikuldottjarmu> Kikuldottjarmus { get; set; } = new List<Kikuldottjarmu>();
}
