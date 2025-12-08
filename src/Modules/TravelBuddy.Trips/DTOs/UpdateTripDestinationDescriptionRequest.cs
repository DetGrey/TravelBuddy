using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Trips.DTOs
{
    public class UpdateTripDestinationDescriptionRequest
    {
        [StringLength(255)]
        public string? Description { get; set; }
    }
}
