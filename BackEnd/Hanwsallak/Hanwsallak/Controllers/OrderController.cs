using Hanwsallak.Domain.Commands.Order;
using Hanwsallak.Domain.DTO.Order;
using Hanwsallak.Domain.Queries.Order;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hanwsallak.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController(IMediator mediator) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? throw new UnauthorizedAccessException());
            
            var command = new CreateOrderCommand
            {
                CreateOrderDto = createOrderDto,
                UserId = userId
            };

            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("MyOrders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? throw new UnauthorizedAccessException());
            
            var query = new GetMyOrdersQuery { UserId = userId };
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}/Accept")]
        public async Task<IActionResult> AcceptOrder(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? throw new UnauthorizedAccessException());
            
            var command = new AcceptOrderCommand
            {
                OrderId = id,
                UserId = userId
            };

            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}/StartDelivery")]
        public async Task<IActionResult> StartDelivery(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? throw new UnauthorizedAccessException());
            
            var command = new StartDeliveryCommand
            {
                OrderId = id,
                UserId = userId
            };

            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}/MarkDelivered")]
        public async Task<IActionResult> MarkDelivered(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? throw new UnauthorizedAccessException());
            
            var command = new MarkDeliveredCommand
            {
                OrderId = id,
                UserId = userId
            };

            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}

