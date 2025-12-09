using Microsoft.AspNetCore.Mvc;

namespace TravelBuddy.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Root()
        {
            return Ok(new
            {
                message = "Welcome to TravelBuddy",
                swaggerUrl = "/swagger",
                timestamp = DateTime.UtcNow
            });
        }

        [HttpGet]
        [Route("api/health")]
        public IActionResult Health()
        {
            return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        }
    }
}
