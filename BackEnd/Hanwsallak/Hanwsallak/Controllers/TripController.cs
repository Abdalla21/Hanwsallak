using Hanwsallak.Domain.Commands.Trip;
using Hanwsallak.Domain.DTO.Trip;
using Hanwsallak.Domain.Queries.Trip;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hanwsallak.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TripController(IMediator mediator) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripDto createTripDto)
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? throw new UnauthorizedAccessException());
            
            var command = new CreateTripCommand
            {
                CreateTripDto = createTripDto,
                UserId = userId
            };

            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("MyTrips")]
        public async Task<IActionResult> GetMyTrips()
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? throw new UnauthorizedAccessException());
            
            var query = new GetMyTripsQuery { UserId = userId };
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("Available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableTrips([FromQuery] string? fromCity, [FromQuery] string? toCity)
        {
            var query = new GetAvailableTripsQuery
            {
                FromCity = fromCity,
                ToCity = toCity
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrip(Guid id, [FromBody] UpdateTripDto updateTripDto)
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? throw new UnauthorizedAccessException());
            
            updateTripDto.Id = id;
            var command = new UpdateTripCommand
            {
                UpdateTripDto = updateTripDto,
                UserId = userId
            };

            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? throw new UnauthorizedAccessException());
            
            var command = new DeleteTripCommand
            {
                TripId = id,
                UserId = userId
            };

            var result = await mediator.Send(command);
            if (result)
                return Ok(new { message = "Trip deleted successfully" });
            return NotFound(new { message = "Trip not found" });
        }
    }
}

