using MediatR;

namespace Hanwsallak.Domain.Commands.Trip
{
    public class DeleteTripCommand : IRequest<bool>
    {
        public Guid TripId { get; set; }
        public Guid UserId { get; set; }
    }
}

