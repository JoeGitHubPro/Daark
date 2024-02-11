using Daark.Entities.Identity.Models;
using System;
using System.Collections.Generic;

namespace Daark.Entities;

public partial class DaarkRealEstate
{
    public int Id { get; set; }

    public DateTime? Date { get; set; }

    public string? UserId { get; set; }

    public int? PortalsId { get; set; }

    public int? LeadsId { get; set; }

    public int? Calls { get; set; }

    public int? FollowUp { get; set; }

    public int? Meeting { get; set; }

    public int? Deal { get; set; }

    public int? LeedsInSheet { get; set; }
    public string? ThingsIDidToday { get; set; }

    public virtual Lead? Leads { get; set; }

    public virtual Portal? Portals { get; set; }

    public virtual ApplicationUser? User { get; set; }
}
