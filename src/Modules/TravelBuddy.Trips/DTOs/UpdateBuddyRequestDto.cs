using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Trips.DTOs
{
    public class UpdateBuddyRequestDto
    {
        public int UserId { get; set; }
        [Required]
        public int BuddyId { get; set; }
        [Required]
        public BuddyRequestUpdateStatus NewStatus { get; set; }
    }
}