using Hanwsallak.Domain.DTO.Trip;
using MediatR;

namespace Hanwsallak.Domain.Queries.Trip
{
    public class GetAvailableTripsQuery : IRequest<List<TripResponseDto>>
    {
        public string? FromCity { get; set; }
        public string? ToCity { get; set; }
    }
}

