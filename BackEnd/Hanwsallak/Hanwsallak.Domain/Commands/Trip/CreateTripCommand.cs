using Hanwsallak.Domain.DTO.Trip;
using MediatR;

namespace Hanwsallak.Domain.Commands.Trip
{
    public class CreateTripCommand : IRequest<TripResponseDto>
    {
        public CreateTripDto CreateTripDto { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}

