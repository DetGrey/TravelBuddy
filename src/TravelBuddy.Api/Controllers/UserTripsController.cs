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
        // This is the dependency on the business logic layer (UserService).
        private readonly ITripDestinationService _tripDestinationService;
        private readonly IBuddyService _buddyService;


        // Constructor: ASP.NET Core automatically injects the IUserService implementation here.
        public UserTripsController(
            ITripDestinationService tripDestinationService,
            IBuddyService buddyService
        )
        {
            _tripDestinationService = tripDestinationService;
            _buddyService = buddyService;
        }

        // GET /api/users/{userId}/trips/trip-destinations
        // The "{userId}" parameter in the HttpGet attribute maps the URL segment to the 'userId' parameter below.
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
        [HttpPost("buddy-request")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PostBuddyRequest([FromRoute] int userId, [FromBody] BuddyDto dto)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            dto.UserId = userId;

            var success = await _buddyService.InsertBuddyRequestAsync(dto);
            if (!success) return BadRequest("Buddy request failed");

            return Created();
        }

        [Authorize]
        [HttpGet("pending-buddy-requests")]
        [ProducesResponseType(typeof(IEnumerable<PendingBuddyRequestsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPendingBuddyRequests([FromRoute] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var pendingBuddyRequests = await _buddyService.GetPendingBuddyRequestsAsync(userId);
            if (!pendingBuddyRequests.Any()) return NoContent();

            return Ok(pendingBuddyRequests);
        }
    }
}