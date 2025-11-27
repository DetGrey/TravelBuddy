using System;
using System.Collections.Generic;

namespace TravelBuddy.SharedKernel.Models;

public partial class SystemEventLog
{
    public int EventId { get; set; }

    public string EventType { get; set; } = null!;

    public int? AffectedId { get; set; }

    public DateTime? TriggeredAt { get; set; }

    public string? Details { get; set; }
}
