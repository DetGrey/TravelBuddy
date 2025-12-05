using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Trips.DTOs;
public class CreateTripDto
{
    [Required]
    public int OwnerId { get; set; }
    [Required]
    public string TripName { get; set; } = string.Empty;
    [Required]
    [Range(1, int.MaxValue)]
    public int MaxBuddies { get; set; }
    [Required]
    public DateOnly StartDate { get; set; }
    [Required]
    public DateOnly EndDate { get; set; }
    public string? Description { get; set; }
    [Required]
    public int ChangedBy { get; set; }
}