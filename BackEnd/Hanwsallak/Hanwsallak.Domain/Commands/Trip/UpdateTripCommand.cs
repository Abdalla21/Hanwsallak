using Hanwsallak.Domain.DTO.Trip;
using MediatR;

namespace Hanwsallak.Domain.Commands.Trip
{
    public class UpdateTripCommand : IRequest<TripResponseDto>
    {
        public UpdateTripDto UpdateTripDto { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}

