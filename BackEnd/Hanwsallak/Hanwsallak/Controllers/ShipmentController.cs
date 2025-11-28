using Hanwsallak.Domain.Commands.Shipment;
using Hanwsallak.Domain.DTO.Shipment;
using Hanwsallak.Domain.Queries.Shipment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hanwsallak.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShipmentController(IMediator mediator) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentDto createShipmentDto)
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? throw new UnauthorizedAccessException());
            
            var command = new CreateShipmentCommand
            {
                CreateShipmentDto = createShipmentDto,
                UserId = userId
            };

            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("MyShipments")]
        public async Task<IActionResult> GetMyShipments()
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? throw new UnauthorizedAccessException());
            
            var query = new GetMyShipmentsQuery { UserId = userId };
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("Available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableShipments([FromQuery] string? fromCity, [FromQuery] string? toCity)
        {
            var query = new GetAvailableShipmentsQuery
            {
                FromCity = fromCity,
                ToCity = toCity
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipment(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? throw new UnauthorizedAccessException());
            
            var command = new DeleteShipmentCommand
            {
                ShipmentId = id,
                UserId = userId
            };

            var result = await mediator.Send(command);
            if (result)
                return Ok(new { message = "Shipment deleted successfully" });
            return NotFound(new { message = "Shipment not found" });
        }
    }
}

