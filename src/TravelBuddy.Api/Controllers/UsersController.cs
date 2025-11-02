using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TravelBuddy.Users;
using TravelBuddy.Trips;
using TravelBuddy.Api.Auth;
using TravelBuddy.Users.DTOs;
using System.Security.Claims;

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
        private readonly JwtTokenGenerator _jwtTokenGenerator;


        // Constructor: ASP.NET Core automatically injects the IUserService implementation here.
        public UsersController(
            IUserService userService,
            ITripDestinationService tripDestinationService,
            JwtTokenGenerator jwtTokenGenerator
        )
        {
            _userService = userService;
            _tripDestinationService = tripDestinationService;
             _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            if (user == null) return Unauthorized("Invalid credentials");

            var token = _jwtTokenGenerator.GenerateToken(user);
            if (string.IsNullOrEmpty(token))
                return StatusCode(StatusCodes.Status500InternalServerError, "Token generation failed.");
            
            var userDto = new UserDto(
                user.UserId,
                user.Name,
                user.Email,
                user.Birthdate
            );

            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return Ok(new { user = userDto, token });
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
    
            var user = await _userService.RegisterAsync(request);
            if (user == null) return Conflict("Email already in use");

            var token = _jwtTokenGenerator.GenerateToken(user);
            if (string.IsNullOrEmpty(token))
                return StatusCode(StatusCodes.Status500InternalServerError, "Token generation failed.");
            
            var userDto = new UserDto(
                user.UserId,
                user.Name,
                user.Email,
                user.Birthdate
            );

            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return Ok(new { user = userDto, token });
        }

        [Authorize]
        [HttpPost("{id}/change-password")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> ChangePassword([FromRoute] int id, [FromBody] PasswordChangeRequestDto request)
        {
            // Note: this means that admin can change other people's passwords
            // This is not ideal for a real project but used here to access our generated data
            if (!User.IsSelfOrAdmin(id))
                return Forbid();

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null)
                return NotFound("User email not found in claims.");

            var success = await _userService.ChangePasswordAsync(request, userEmail, id);
            if (!success)
                 return BadRequest("Password change failed due to invalid input or policy violation.");

            return Ok("Password changed successfully.");
        }

        [Authorize(Roles = "admin")]
        // [HttpGet] maps this method to an HTTP GET request (URL: /api/users).
        [HttpGet]
        // [ProducesResponseType] is for documentation (Swagger/OpenAPI).
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
        [Authorize]
        [HttpGet("{id}/trip-destinations")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserDto>> GetUserTrips([FromRoute] int id)
        {
            if (!User.IsSelfOrAdmin(id))
                return Forbid();

            var tripDestinations = await _tripDestinationService.GetUserTripsAsync(id);
            if (!tripDestinations.Any()) return NoContent();

            return Ok(tripDestinations);
        }
        
        [Authorize(Roles = "admin")]
        [HttpGet("admin-action")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult AdminOnlyStuff() => Ok("Admins only!");

    }
}