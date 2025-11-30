using TravelBuddy.Users.Models;

namespace TravelBuddy.Trips.Models;

public partial class Trip
{
    public int TripId { get; set; }

    public int? OwnerId { get; set; }
    
    public string? TripName { get; set; }

    public int? MaxBuddies { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string? Description { get; set; }

    public bool? IsArchived { get; set; }

    public virtual User? Owner { get; set; }

    public virtual ICollection<TripAudit> TripAudits { get; set; } = new List<TripAudit>();

    public virtual ICollection<TripDestination> TripDestinations { get; set; } = new List<TripDestination>();
}
