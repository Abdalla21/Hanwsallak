using Hanwsallak.Domain.DTO.Trip;
using Hanwsallak.Domain.Queries.Trip;
using Hanwsallak.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hanwsallak.Infrastructure.Handlers.Queries.Trip
{
    public class GetMyTripsQueryHandler : IRequestHandler<GetMyTripsQuery, List<TripResponseDto>>
    {
        private readonly ReadOnlyDBContext _context;

        public GetMyTripsQueryHandler(ReadOnlyDBContext context)
        {
            _context = context;
        }

        public async Task<List<TripResponseDto>> Handle(GetMyTripsQuery request, CancellationToken cancellationToken)
        {
            var trips = await _context.Trips
                .Include(t => t.Traveler)
                .Where(t => t.TravelerId == request.UserId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);

            return trips.Select(t => new TripResponseDto
            {
                Id = t.Id,
                TravelerId = t.TravelerId,
                TravelerName = t.Traveler?.FullName ?? t.Traveler?.UserName ?? "Unknown",
                FromCity = t.FromCity,
                ToCity = t.ToCity,
                DepartureDate = t.DepartureDate,
                DepartureTime = t.DepartureTime,
                RecurringDay = t.RecurringDay,
                AvailableCapacity = t.AvailableCapacity,
                MaxPackages = t.MaxPackages,
                Status = t.Status,
                CreatedAt = t.CreatedAt
            }).ToList();
        }
    }
}

