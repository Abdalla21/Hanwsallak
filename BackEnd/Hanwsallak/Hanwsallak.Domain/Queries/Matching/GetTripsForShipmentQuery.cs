using Hanwsallak.Domain.DTO.Trip;
using MediatR;

namespace Hanwsallak.Domain.Queries.Matching
{
    public class GetTripsForShipmentQuery : IRequest<List<TripResponseDto>>
    {
        public Guid ShipmentId { get; set; }
    }
}

