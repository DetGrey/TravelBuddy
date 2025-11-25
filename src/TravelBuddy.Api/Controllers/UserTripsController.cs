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
        private readonly ITripDestinationService _tripDestinationService;

        public UserTripsController(
            ITripDestinationService tripDestinationService
        )
        {
            _tripDestinationService = tripDestinationService;
        }

        [Authorize]
        [HttpGet("trip-destinations")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserTrips([FromRoute] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var tripDestinations = await _tripDestinationService.GetUserTripsAsync(userId);
            if (!tripDestinations.Any()) return NoContent();

            return Ok(tripDestinations);
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
            var triggeredBy = User.GetUserId();
            if (triggeredBy == null) return Unauthorized();

            var isSelfOrAdmin = User.IsSelfOrAdmin(userId);
            var isOwner = await _tripDestinationService.IsTripOwnerAsync(triggeredBy.Value, tripDestinationId);

            if (!isSelfOrAdmin && !isOwner)
                return Forbid();

            if (string.IsNullOrWhiteSpace(departureReason))
            {
                departureReason = isSelfOrAdmin
                    ? "Left voluntarily or by admin"
                    : "Removed by owner";
            }

            var (success, errorMessage) = await _tripDestinationService.LeaveTripDestinationAsync(
                userId,
                tripDestinationId,
                triggeredBy.Value,
                departureReason
            );

            if (!success) 
            {
                return BadRequest(errorMessage ?? "Failed to update buddy status.");
            }

            return NoContent();
        }
    }
}