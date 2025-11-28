using Hanwsallak.Domain.Commands.Trip;
using Hanwsallak.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hanwsallak.Infrastructure.Handlers.Commands.Trip
{
    public class DeleteTripCommandHandler : IRequestHandler<DeleteTripCommand, bool>
    {
        private readonly ApplicationDBContext _context;

        public DeleteTripCommandHandler(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteTripCommand request, CancellationToken cancellationToken)
        {
            var trip = await _context.Trips
                .FirstOrDefaultAsync(t => t.Id == request.TripId && t.TravelerId == request.UserId, cancellationToken);

            if (trip == null)
                return false;

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

