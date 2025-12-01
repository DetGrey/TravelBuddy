using Microsoft.AspNetCore.Mvc;
using TravelBuddy.Trips;
using TravelBuddy.Trips.DTOs;
using TravelBuddy.Api.Auth;
using TravelBuddy.Trips.Models;

namespace TravelBuddy.Api.Controllers
{
    [ApiController]
    [Route("api/destinations")] 
    public class DestinationsController : ControllerBase
    {
        private readonly ITripService _tripService;

        // Constructor: ASP.NET Core automatically injects the ITripDestinationService implementation here.
        public DestinationsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<DestinationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<DestinationDto>>> GetDestinations()
        {
            var trips = await _tripService.GetDestinationsAsync();

            if (trips == null || !trips.Any())
                return NoContent();

            return Ok(trips);
        }
    }
}