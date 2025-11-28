using Hanwsallak.Domain.DTO.Shipment;
using MediatR;

namespace Hanwsallak.Domain.Queries.Shipment
{
    public class GetAvailableShipmentsQuery : IRequest<List<ShipmentResponseDto>>
    {
        public string? FromCity { get; set; }
        public string? ToCity { get; set; }
    }
}

