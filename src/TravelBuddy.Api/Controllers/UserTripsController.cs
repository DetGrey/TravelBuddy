using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TravelBuddy.Api.Auth;
using TravelBuddy.Users;
using TravelBuddy.Users.DTOs;
using TravelBuddy.Trips;
using TravelBuddy.Trips.DTOs;
using TravelBuddy.Api.Services;

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

        // New buddy trips endpoint with weather integration
        [Authorize]
        [HttpGet("trip-destinations/buddy/weather")]
        [ProducesResponseType(typeof(IEnumerable<BuddyTripWithWeatherDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBuddyTripsWithWeather(
            [FromRoute] int userId, 
            [FromServices] IWeatherService weatherService)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            // 1. Get the trips from DB
            var trips = await _tripService.GetBuddyTripsAsync(userId);
            if (trips == null || !trips.Any()) 
                return Ok(new List<BuddyTripWithWeatherDto>());

            // 1. Create a "Bouncer" that only lets 1 request pass at a time
            // (Using 1 is safest for Free Tier. You can try 2, but 1 is guaranteed to work)
            using SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

            var tripTasks = trips.Select(async trip => 
            {
                WeatherSummary? weather = null;
                if (!string.IsNullOrWhiteSpace(trip.DestinationName))
                {
                    // 2. Wait for your turn to enter
                    await _semaphore.WaitAsync(); 
                    try
                    {
                        weather = await weatherService.GetWeatherForTripAsync(
                            trip.DestinationName, 
                            trip.StartDate, 
                            trip.EndDate
                        );
                        
                        // 3. Add a tiny polite delay between calls (e.g., 200ms)
                        // This ensures you don't hit the "Queries Per Second" limit
                        await Task.Delay(100); 
                    }
                    finally
                    {
                        // 4. Release the lock so the next trip can go
                        _semaphore.Release();
                    }
                }
                return new BuddyTripWithWeatherDto(trip, weather);
            });

            var result = await Task.WhenAll(tripTasks);

            return Ok(result);
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

        [Authorize]
        [HttpPatch("{tripId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTripInfo(
            [FromRoute] int userId,
            [FromRoute] int tripId,
            [FromBody] UpdateTripInfoRequest request
        )
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var changedBy = User.GetUserId();
            if (changedBy == null) return Unauthorized();

            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var (success, errorMessage) = await _tripService.UpdateTripInfoAsync(
                tripId,
                userId,
                request.TripName,
                request.Description
            );

            if (!success)
            {
                if (errorMessage?.Contains("not found") == true) return NotFound(new { error = errorMessage });
                if (errorMessage?.Contains("not the owner") == true) return Forbid();
                return BadRequest(new { error = errorMessage ?? "Failed to update trip information." });
            }

            return NoContent();
        }

        [Authorize]
        [HttpPatch("trip-destinations/{tripDestinationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTripDestinationDescription(
            [FromRoute] int userId,
            [FromRoute] int tripDestinationId,
            [FromBody] UpdateTripDestinationDescriptionRequest request
        )
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var changedBy = User.GetUserId();
            if (changedBy == null) return Unauthorized();

            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var (success, errorMessage) = await _tripService.UpdateTripDestinationDescriptionAsync(
                tripDestinationId,
                userId,
                request.Description
            );

            if (!success)
            {
                if (errorMessage?.Contains("not found") == true) return NotFound(new { error = errorMessage });
                if (errorMessage?.Contains("not the owner") == true) return Forbid();
                return BadRequest(new { error = errorMessage ?? "Failed to update trip destination description." });
            }

            return NoContent();
        }
    }
}