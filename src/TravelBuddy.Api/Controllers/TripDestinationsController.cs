using Microsoft.AspNetCore.Mvc;
using TravelBuddy.Trips;
using TravelBuddy.Trips.DTOs;
using TravelBuddy.Api.Auth;

namespace TravelBuddy.Api.Controllers
{
    [ApiController]
    [Route("api/trip-destinations")] 
    public class TripDestinationsController : ControllerBase
    {
        private readonly ITripService _tripService;

        // Constructor: ASP.NET Core automatically injects the ITripDestinationService implementation here.
        public TripDestinationsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<TripDestinationDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TripDestinationDto>>> SearchTrips(
            [FromQuery] TripSearchRequest filter)
        {
            var trips = await _tripService.SearchTripsAsync(
                filter.StartDate, 
                filter.EndDate, 
                filter.Country, 
                filter.State, 
                filter.Name, 
                filter.PartySize, 
                filter.Query);

            return Ok(trips);
        }
    }
}