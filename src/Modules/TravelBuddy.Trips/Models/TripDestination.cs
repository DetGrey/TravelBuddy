using System;
using System.Collections.Generic;

namespace TravelBuddy.Trips.Models;

public partial class TripDestination
{
    public int TripDestinationId { get; set; }

    public int DestinationId { get; set; }

    public int TripId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int SequenceNumber { get; set; }

    public string? Description { get; set; }

    public bool? IsArchived { get; set; }

    public virtual ICollection<Buddy> Buddies { get; set; } = new List<Buddy>();

    public virtual Destination Destination { get; set; } = null!;

    public virtual Trip Trip { get; set; } = null!;
}
