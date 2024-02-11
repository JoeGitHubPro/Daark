using System;
using System.Collections.Generic;

namespace Daark.Entities;

public partial class Bayut
{
    public int Id { get; set; }

    public int? Dubai { get; set; }

    public int? Other { get; set; }

    public virtual ICollection<Portal> Portals { get; set; } = new List<Portal>();
}
