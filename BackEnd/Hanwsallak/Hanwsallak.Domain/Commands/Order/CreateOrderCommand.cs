using Hanwsallak.Domain.DTO.Order;
using MediatR;

namespace Hanwsallak.Domain.Commands.Order
{
    public class CreateOrderCommand : IRequest<OrderResponseDto>
    {
        public CreateOrderDto CreateOrderDto { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}

