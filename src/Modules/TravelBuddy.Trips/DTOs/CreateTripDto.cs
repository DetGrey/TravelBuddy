namespace TravelBuddy.Trips.DTOs;
public class CreateTripDto
{
    public int OwnerId { get; set; }
    public string TripName { get; set; } = string.Empty;
    public int MaxBuddies { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? Description { get; set; }
    public int ChangedBy { get; set; }
}