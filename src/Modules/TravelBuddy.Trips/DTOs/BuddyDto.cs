using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Trips.DTOs
{
    public class BuddyDto
    {
        public int UserId { get; set; }
        [Required]
        public int TripDestinationId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "PersonCount must be at least 1")]
        public int PersonCount { get; set; }
        [MaxLength(255)]
        public string? Note { get; set; }
    }
}