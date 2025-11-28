using Hanwsallak.Domain.DTO.Shipment;
using MediatR;

namespace Hanwsallak.Domain.Queries.Shipment
{
    public class GetMyShipmentsQuery : IRequest<List<ShipmentResponseDto>>
    {
        public Guid UserId { get; set; }
    }
}

