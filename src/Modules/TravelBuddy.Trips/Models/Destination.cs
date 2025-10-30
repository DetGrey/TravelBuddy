using System;
using System.Collections.Generic;

namespace TravelBuddy.Trips.Models;

public partial class Destination
{
    public int DestinationId { get; set; }

    public string Name { get; set; } = null!;

    public string? State { get; set; }

    public string Country { get; set; } = null!;

    public decimal? Longitude { get; set; }

    public decimal? Latitude { get; set; }

    public virtual ICollection<TripDestination> TripDestinations { get; set; } = new List<TripDestination>();
}
