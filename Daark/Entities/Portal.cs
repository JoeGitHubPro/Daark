using System;
using System.Collections.Generic;

namespace Daark.Entities;

public partial class Portal
{
    public int Id { get; set; }

    public int? BayutId { get; set; }

    public int? PropertyFinderId { get; set; }

    public int? Semsar { get; set; }

    public virtual Bayut? Bayut { get; set; }

    public virtual ICollection<DaarkRealEstate> DaarkRealEstates { get; set; } = new List<DaarkRealEstate>();

    public virtual PropertyFinder? PropertyFinder { get; set; }
}
