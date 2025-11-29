namespace TravelBuddy.Trips.Models;

public class TripHeaderInfo
{
    public int TripId { get; set; }
    public string TripName { get; set; } = null!;
    public DateTime TripStartDate { get; set; }
    public DateTime TripEndDate { get; set; }
    public int MaxBuddies { get; set; }
    public string TripDescription { get; set; } = null!;
    public int OwnerUserId { get; set; }
    public string OwnerName { get; set; } = null!;
}