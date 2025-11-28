using Hanwsallak.Domain.Queries.Matching;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hanwsallak.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MatchingController(IMediator mediator) : ControllerBase
    {
        [HttpGet("ShipmentsForTrip/{tripId}")]
        public async Task<IActionResult> GetShipmentsForTrip(Guid tripId)
        {
            var query = new GetShipmentsForTripQuery { TripId = tripId };
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("TripsForShipment/{shipmentId}")]
        public async Task<IActionResult> GetTripsForShipment(Guid shipmentId)
        {
            var query = new GetTripsForShipmentQuery { ShipmentId = shipmentId };
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}

