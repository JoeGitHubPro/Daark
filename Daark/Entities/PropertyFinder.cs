using System;
using System.Collections.Generic;

namespace Daark.Entities;

public partial class PropertyFinder
{
    public int Id { get; set; }

    public int? Dubai { get; set; }

    public int? Rak { get; set; }

    public virtual ICollection<Portal> Portals { get; set; } = new List<Portal>();
}
