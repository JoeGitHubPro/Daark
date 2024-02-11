using Daark.Entities.Identity.Models;
using System;
using System.Collections.Generic;

namespace Daark.Entities;

public partial class UserTeam
{
    [Key]
    public int UserTeamId { get; set; }

    public string UserId { get; set; } = null!;

    public int TeamId { get; set; }

    public virtual Team Team { get; set; } = null!;

    public virtual ApplicationUser User { get; set; } = null!;
}
