using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Trips.DTOs;
public class CreateTripDto
{
    [Required]
    public int OwnerId { get; set; }
    
    [Required]
    [RegularExpression(@"^.*\S.*$", ErrorMessage = "Trip name cannot be empty or whitespace.")]
    [MaxLength(100, ErrorMessage = "Trip name cannot exceed 100 characters")]
    public string TripName { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int MaxBuddies { get; set; }

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    [MaxLength(255, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }
    
    [Required]
    public int ChangedBy { get; set; }
}