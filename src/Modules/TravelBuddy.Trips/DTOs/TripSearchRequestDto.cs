using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Trips.DTOs;
public class TripSearchRequest
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    
    [MaxLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
    public string? Country { get; set; }
    [MaxLength(100, ErrorMessage = "State cannot exceed 100 characters")]
    public string? State { get; set; }
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string? Name { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Party size must be at least 1")]
    public int? PartySize { get; set; }
    
    // This attribute maps ?q=... in the URL to this property.
    [FromQuery(Name = "q")] 
    public string? Query { get; set; }
}