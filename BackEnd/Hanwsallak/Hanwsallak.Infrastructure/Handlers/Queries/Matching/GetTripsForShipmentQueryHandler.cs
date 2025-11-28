using Hanwsallak.Domain.DTO.Trip;
using Hanwsallak.Domain.Queries.Matching;
using Hanwsallak.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hanwsallak.Infrastructure.Handlers.Queries.Matching
{
    public class GetTripsForShipmentQueryHandler : IRequestHandler<GetTripsForShipmentQuery, List<TripResponseDto>>
    {
        private readonly ReadOnlyDBContext _context;

        public GetTripsForShipmentQueryHandler(ReadOnlyDBContext context)
        {
            _context = context;
        }

        public async Task<List<TripResponseDto>> Handle(GetTripsForShipmentQuery request, CancellationToken cancellationToken)
        {
            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);

            if (shipment == null)
                return new List<TripResponseDto>();

            var trips = await _context.Trips
                .Include(t => t.Traveler)
                .Where(t => t.Status == "Available" 
                    && t.FromCity == shipment.FromCity 
                    && t.ToCity == shipment.ToCity
                    && t.AvailableCapacity >= shipment.Weight
                    && t.DepartureDate >= DateTime.UtcNow.Date)
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

