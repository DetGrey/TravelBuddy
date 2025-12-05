using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Trips.DTOs;
public class CreateTripDestinationDto
{
    public int? TripId { get; set; }
    [Required]
    public DateOnly DestinationStartDate { get; set; }
    [Required]
    public DateOnly DestinationEndDate { get; set; }
    [Required]
    public int SequenceNumber { get; set; }
    public string? Description { get; set; }

    // Either provide DestinationId OR fill in new destination fields
    public int? DestinationId { get; set; }

    // New destination fields (used if DestinationId is null)
    public string? Name { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
}