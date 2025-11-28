using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hanwsallak.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new
            {
                message = "Welcome to Hanwsallak API",
                version = "1.0.0",
                status = "Running",
                endpoints = new
                {
                    authentication = "/api/Authentication",
                    trips = "/api/Trip",
                    shipments = "/api/Shipment",
                    orders = "/api/Order",
                    matching = "/api/Matching"
                }
            });
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
