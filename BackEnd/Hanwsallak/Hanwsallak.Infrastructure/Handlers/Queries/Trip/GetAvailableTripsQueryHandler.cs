using Hanwsallak.Domain.DTO.Trip;
using Hanwsallak.Domain.Queries.Trip;
using Hanwsallak.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hanwsallak.Infrastructure.Handlers.Queries.Trip
{
    public class GetAvailableTripsQueryHandler : IRequestHandler<GetAvailableTripsQuery, List<TripResponseDto>>
    {
        private readonly ReadOnlyDBContext _context;

        public GetAvailableTripsQueryHandler(ReadOnlyDBContext context)
        {
            _context = context;
        }

        public async Task<List<TripResponseDto>> Handle(GetAvailableTripsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Trips
                .Include(t => t.Traveler)
                .Where(t => t.Status == "Available" && t.DepartureDate >= DateTime.UtcNow.Date);

            if (!string.IsNullOrEmpty(request.FromCity))
                query = query.Where(t => t.FromCity.Contains(request.FromCity));

            if (!string.IsNullOrEmpty(request.ToCity))
                query = query.Where(t => t.ToCity.Contains(request.ToCity));

            var trips = await query
                .OrderBy(t => t.DepartureDate)
                .ThenBy(t => t.DepartureTime)
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

