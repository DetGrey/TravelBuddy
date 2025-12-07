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
    [RegularExpression(@"^.*\S.*$", ErrorMessage = "Name cannot be empty or whitespace.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string? Name { get; set; }

    [MaxLength(100, ErrorMessage = "State cannot exceed 100 characters")]
    public string? State { get; set; }

    [RegularExpression(@"^.*\S.*$", ErrorMessage = "Country cannot be empty or whitespace.")]
    [MaxLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
    public string? Country { get; set; }

    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180 degrees")]
    public decimal? Longitude { get; set; }

    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90 degrees")]
    public decimal? Latitude { get; set; }
}