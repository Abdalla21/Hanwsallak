using Hanwsallak.Domain.DTO.Order;
using MediatR;

namespace Hanwsallak.Domain.Queries.Order
{
    public class GetMyOrdersQuery : IRequest<List<OrderResponseDto>>
    {
        public Guid UserId { get; set; }
    }
}

