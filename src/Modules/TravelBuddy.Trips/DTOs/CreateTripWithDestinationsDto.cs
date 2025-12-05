using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Trips.DTOs;
public class CreateTripWithDestinationsDto
{
    [Required]
    public CreateTripDto CreateTrip { get; set; } = new CreateTripDto();
    [Required]
    [MinLength(1)] // At least one destination is required
    public List<CreateTripDestinationDto> TripDestinations { get; set; } = new List<CreateTripDestinationDto>();
}