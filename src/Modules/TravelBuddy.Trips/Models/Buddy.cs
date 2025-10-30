using System;
using System.Collections.Generic;
using TravelBuddy.Users.Models;

namespace TravelBuddy.Trips.Models;

public partial class Buddy
{
    public int BuddyId { get; set; }

    public int UserId { get; set; }

    public int TripDestinationId { get; set; }

    public int? PersonCount { get; set; }

    public string? Note { get; set; }

    public bool? IsActive { get; set; }

    public string? DepartureReason { get; set; }

    public string RequestStatus { get; set; } = null!;

    public virtual ICollection<BuddyAudit> BuddyAudits { get; set; } = new List<BuddyAudit>();

    public virtual TripDestination TripDestination { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
