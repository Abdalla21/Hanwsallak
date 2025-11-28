using Hanwsallak.Domain.DTO.Order;
using MediatR;

namespace Hanwsallak.Domain.Commands.Order
{
    public class StartDeliveryCommand : IRequest<OrderResponseDto>
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
    }
}

