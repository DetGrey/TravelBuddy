using System;
using System.Collections.Generic;
using TravelBuddy.Users.Models;

namespace TravelBuddy.Trips.Models;

public partial class TripAudit
{
    public int AuditId { get; set; }

    public int TripId { get; set; }

    public string Action { get; set; } = null!;

    public string? FieldChanged { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public int? ChangedBy { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual User? ChangedByNavigation { get; set; }

    public virtual Trip Trip { get; set; } = null!;
}
