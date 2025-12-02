using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TravelBuddy.Api.Auth;
using TravelBuddy.Users;
using TravelBuddy.Users.DTOs;
using TravelBuddy.Trips;
using TravelBuddy.Trips.DTOs;

namespace TravelBuddy.Api.Controllers
{
    [ApiController]
    // [Route] defines the base URL for this controller
    [Route("api/users/{userId}/trips")]
    public class UserTripsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public UserTripsController(
            ITripService tripDestinationService
        )
        {
            _tripService = tripDestinationService;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateTripWithTripDestinations(
            [FromRoute] int userId,
            [FromBody] CreateTripWithDestinationsDto createTripWithDestinationsDto
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (!User.IsSelfOrAdmin(userId)) return Forbid();


            int? changedById = User.GetUserId();
            if (changedById != null) {
                createTripWithDestinationsDto.CreateTrip.ChangedBy = changedById.Value;
            }

            // Check that for new destinations, either destinationId is provided 
            // or all new destination fields are filled
            if (createTripWithDestinationsDto.TripDestinations.Any(td => 
                td.DestinationId == null &&
                (string.IsNullOrWhiteSpace(td.Name) ||
                 string.IsNullOrWhiteSpace(td.Country) ||
                 td.Longitude == null ||
                 td.Latitude == null)))
            {
                return BadRequest("New destination fields are incomplete.");
            }

            var (success, errorMessage) = await _tripService
                .CreateTripWithDestinationsAsync(createTripWithDestinationsDto);
            
            if (!success)
            {
                return BadRequest(errorMessage ?? "Failed to create trip.");
            }

            return Created();
        }

        [Authorize]
        [HttpGet("{tripId}")]
        [ProducesResponseType(typeof(TripOverviewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetFullTripOverview([FromRoute] int userId, [FromRoute] int tripId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var tripOverview = await _tripService.GetFullTripOverviewAsync(tripId);
            if (tripOverview == null) return NoContent();

            return Ok(tripOverview);
        }

        [Authorize]
        [HttpGet("trip-destinations/buddy")]
        [ProducesResponseType(typeof(IEnumerable<BuddyTripSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetBuddyTrips([FromRoute] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var tripDestinations = await _tripService.GetBuddyTripsAsync(userId);
            if (!tripDestinations.Any()) return NoContent();

            return Ok(tripDestinations);
        }
        [Authorize]
        [HttpGet("trip-destinations/owned")]
        [ProducesResponseType(typeof(IEnumerable<TripOverviewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetOwnedTrips([FromRoute] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var tripDestinations = await _tripService.GetOwnedTripOverviewsAsync(userId);
            if (!tripDestinations.Any()) return NoContent();

            return Ok(tripDestinations);
        }
        [Authorize]
        [HttpGet("trip-destinations/{tripDestinationId}")]
        [ProducesResponseType(typeof(TripDestinationInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetTripDestinationInfo([FromRoute] int userId, [FromRoute] int tripDestinationId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var tripDestination = await _tripService.GetTripDestinationInfoAsync(tripDestinationId);
            if (tripDestination == null) return NoContent();

            return Ok(tripDestination);
        }

        [Authorize]
        [HttpDelete("trip-destinations/{tripDestinationId}/leave")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> LeaveTripDestination(
            [FromRoute] int userId,
            [FromRoute] int tripDestinationId,
            [FromQuery] string? departureReason
        ) {
            var changedBy = User.GetUserId();
            if (changedBy == null) return Unauthorized();

            var isSelfOrAdmin = User.IsSelfOrAdmin(userId);
            var isOwner = await _tripService.IsTripOwnerAsync(changedBy.Value, tripDestinationId);

            if (!isSelfOrAdmin && !isOwner)
                return Forbid();

            if (string.IsNullOrWhiteSpace(departureReason))
            {
                departureReason = isSelfOrAdmin
                    ? "Left voluntarily or by admin"
                    : "Removed by owner";
            }

            var (success, errorMessage) = await _tripService.LeaveTripDestinationAsync(
                userId,
                tripDestinationId,
                changedBy.Value,
                departureReason
            );
            // TODO should this be BadRequest or something else?
            if (!success) 
            {
                return BadRequest(errorMessage ?? "Failed to update buddy status.");
            }

            return NoContent();
        }
    }
}