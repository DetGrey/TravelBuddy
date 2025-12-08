using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Trips.DTOs
{
    public class UpdateTripInfoRequest
    {
        [StringLength(100, MinimumLength = 1)]
        [RegularExpression(@"^.*\S.*$", ErrorMessage = "Trip name cannot be empty or whitespace.")]
        public string? TripName { get; set; } = null!;

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
