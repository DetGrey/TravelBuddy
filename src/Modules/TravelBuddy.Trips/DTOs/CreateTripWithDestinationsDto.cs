using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Trips.DTOs;
public class CreateTripWithDestinationsDto
{
    [Required]
    public CreateTripDto CreateTrip { get; set; } = new CreateTripDto();
    [Required]
    public List<CreateTripDestinationDto> TripDestinations { get; set; } = new List<CreateTripDestinationDto>();
}