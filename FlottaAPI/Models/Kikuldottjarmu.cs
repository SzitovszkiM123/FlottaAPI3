using System;
using System.Collections.Generic;

namespace FlottaAPI.Models;

public partial class Kikuldottjarmu
{
    public int Id { get; set; }

    public int GepjarmuId { get; set; }

    public int KikuldetesId { get; set; }

    public int Sofor { get; set; }

    public virtual Gepjarmu Gepjarmu { get; set; } = null!;

    public virtual Kikuldete Kikuldetes { get; set; } = null!;

    public virtual Sofor SoforNavigation { get; set; } = null!;
}
