using Hanwsallak.Domain.DTO.Trip;
using MediatR;

namespace Hanwsallak.Domain.Queries.Trip
{
    public class GetMyTripsQuery : IRequest<List<TripResponseDto>>
    {
        public Guid UserId { get; set; }
    }
}

