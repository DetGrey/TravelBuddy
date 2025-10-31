using Microsoft.AspNetCore.Mvc;
using TravelBuddy.Users; // Accesses the IUserService contract and UserDto
using TravelBuddy.Trips;

namespace TravelBuddy.Api.Controllers
{
    // [ApiController] enables automatic API features like model validation.
    [ApiController]
    // [Route] defines the base URL for this controller (e.g., /api/users).
    [Route("api/[controller]")] 
    public class UsersController : ControllerBase
    {
        // This is the dependency on the business logic layer (UserService).
        private readonly IUserService _userService;
        private readonly ITripDestinationService _tripDestinationService;

        // Constructor: ASP.NET Core automatically injects the IUserService implementation here.
        public UsersController(
            IUserService userService,
            ITripDestinationService tripDestinationService
        )
        {
            _userService = userService;
            _tripDestinationService = tripDestinationService;
        }

        // [HttpGet] maps this method to an HTTP GET request (URL: /api/users).
        [HttpGet]
        // [ProducesResponseType] is for documentation (Swagger/OpenAPI).
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            // 1. Call the business service to get the list of safe UserDto objects.
            var users = await _userService.GetAllUsersAsync();

            // 2. Check the result and handle the case where no users are found.
            if (users == null || !users.Any())
            {
                // Returns HTTP 204 No Content.
                return NoContent();
            }

            // 3. Returns the list of users with an HTTP 200 OK status.
            return Ok(users);
        }
        
        // GET /api/users/{id}/trip-destinations
        // The "{id}" parameter in the HttpGet attribute maps the URL segment to the 'id' parameter below.
        [HttpGet("{id}/trip-destinations")]
        [ProducesResponseType(typeof(UserDto), 200)] // Success response type
        [ProducesResponseType(404)]                   // Not Found response
        public async Task<ActionResult<UserDto>> GetUserTrips([FromRoute] int id)
        {
            var tripDestinations = await _tripDestinationService.GetUserTripsAsync(id);
            if (!tripDestinations.Any()) return NoContent();
            return Ok(tripDestinations);
        }
    }
}