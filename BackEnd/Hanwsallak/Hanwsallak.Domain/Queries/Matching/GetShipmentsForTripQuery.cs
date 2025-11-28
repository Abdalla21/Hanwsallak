using Hanwsallak.Domain.DTO.Shipment;
using MediatR;

namespace Hanwsallak.Domain.Queries.Matching
{
    public class GetShipmentsForTripQuery : IRequest<List<ShipmentResponseDto>>
    {
        public Guid TripId { get; set; }
    }
}

