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
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateTripWithTripDestinations(
            [FromRoute] int userId,
            [FromBody] CreateTripWithDestinationsDto dto
        )
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var currentUserId = User.GetUserId();
            if (currentUserId != null) dto.CreateTrip.ChangedBy = currentUserId.Value;

            var (success, errorMessage) = await _tripService.CreateTripWithDestinationsAsync(dto);
            
            if (!success) return BadRequest(new { error = errorMessage ?? "Failed to create trip." });

            return Created();
        }

        [Authorize]
        [HttpGet("{tripId}")]
        [ProducesResponseType(typeof(TripOverviewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetFullTripOverview([FromRoute] int userId, [FromRoute] int tripId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var tripOverview = await _tripService.GetFullTripOverviewAsync(tripId);
            if (tripOverview == null) return NotFound();

            return Ok(tripOverview);
        }

        [Authorize]
        [HttpGet("trip-destinations/buddy")]
        [ProducesResponseType(typeof(IEnumerable<BuddyTripSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetBuddyTrips([FromRoute] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var tripDestinations = await _tripService.GetBuddyTripsAsync(userId);
            return Ok(tripDestinations ?? new List<BuddyTripSummaryDto>());
        }

        [Authorize]
        [HttpGet("trip-destinations/owned")]
        [ProducesResponseType(typeof(IEnumerable<TripOverviewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetOwnedTrips([FromRoute] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var tripDestinations = await _tripService.GetOwnedTripOverviewsAsync(userId);
            return Ok(tripDestinations ?? new List<TripOverviewDto>());
        }
        [Authorize]
        [HttpGet("trip-destinations/{tripDestinationId}")]
        [ProducesResponseType(typeof(TripDestinationInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetTripDestinationInfo([FromRoute] int userId, [FromRoute] int tripDestinationId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var tripDestination = await _tripService.GetTripDestinationInfoAsync(tripDestinationId);
            if (tripDestination == null) return NotFound();

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

            var (success, errorMessage) = await _tripService.LeaveTripDestinationAsync(
                userId,
                tripDestinationId,
                changedBy.Value,
                departureReason,
                isSelfOrAdmin
            );
            
            if (!success) return BadRequest(new { error = errorMessage ?? "Failed to leave trip destination." });

            return NoContent();
        }
    }
}