using Microsoft.AspNetCore.Mvc;

namespace TravelBuddy.Trips.DTOs;
public class TripSearchRequest
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? Name { get; set; }
    public int? PartySize { get; set; }
    
    // This attribute maps ?q=... in the URL to this property.
    [FromQuery(Name = "q")] 
    public string? Query { get; set; }
}