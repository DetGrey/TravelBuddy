namespace TravelBuddy.Trips.Models;

public class TripHeaderInfo
{
    public int TripId { get; set; }
    public string TripName { get; set; } = null!;
    public DateOnly TripStartDate { get; set; }
    public DateOnly TripEndDate { get; set; }
    public int MaxBuddies { get; set; }
    public string TripDescription { get; set; } = null!;
    public int OwnerUserId { get; set; }
    public string OwnerName { get; set; } = null!;
}