using Microsoft.AspNetCore.Mvc;
using TravelBuddy.Trips;
using TravelBuddy.Api.Auth;

namespace TravelBuddy.Api.Controllers
{
    [ApiController]
    [Route("api/trip-destinations")] 
    public class TripDestinationsController : ControllerBase
    {
        private readonly ITripDestinationService _tripDestinationService;

        // Constructor: ASP.NET Core automatically injects the ITripDestinationService implementation here.
        public TripDestinationsController(ITripDestinationService TripDestinationService)
        {
            _tripDestinationService = TripDestinationService;
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<TripDestinationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<TripDestinationDto>>> SearchTrips(
            [FromQuery] DateOnly? reqStart,
            [FromQuery] DateOnly? reqEnd,
            [FromQuery] string? country,
            [FromQuery] string? state,
            [FromQuery] string? name,
            [FromQuery] int? partySize,
            [FromQuery] string? q)
        {
            var trips = await _tripDestinationService.SearchTripsAsync(
                reqStart, reqEnd, country, state, name, partySize, q);

            if (trips == null || !trips.Any())
                return NoContent();

            return Ok(trips);
        }
    }
}