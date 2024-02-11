using System;
using System.Collections.Generic;

namespace Daark.Entities;

public partial class Lead
{
    public int Id { get; set; }

    public int? Bayut { get; set; }

    public int? PropertyFinder { get; set; }

    public int? Semsar { get; set; }

    public virtual ICollection<DaarkRealEstate> DaarkRealEstates { get; set; } = new List<DaarkRealEstate>();
}
