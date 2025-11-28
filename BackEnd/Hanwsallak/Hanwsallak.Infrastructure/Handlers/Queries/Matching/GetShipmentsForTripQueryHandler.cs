using Hanwsallak.Domain.DTO.Shipment;
using Hanwsallak.Domain.Queries.Matching;
using Hanwsallak.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hanwsallak.Infrastructure.Handlers.Queries.Matching
{
    public class GetShipmentsForTripQueryHandler : IRequestHandler<GetShipmentsForTripQuery, List<ShipmentResponseDto>>
    {
        private readonly ReadOnlyDBContext _context;

        public GetShipmentsForTripQueryHandler(ReadOnlyDBContext context)
        {
            _context = context;
        }

        public async Task<List<ShipmentResponseDto>> Handle(GetShipmentsForTripQuery request, CancellationToken cancellationToken)
        {
            var trip = await _context.Trips
                .FirstOrDefaultAsync(t => t.Id == request.TripId, cancellationToken);

            if (trip == null)
                return new List<ShipmentResponseDto>();

            var shipments = await _context.Shipments
                .Include(s => s.Customer)
                .Where(s => s.Status == "Pending" 
                    && s.FromCity == trip.FromCity 
                    && s.ToCity == trip.ToCity
                    && s.Weight <= trip.AvailableCapacity)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync(cancellationToken);

            return shipments.Select(s => new ShipmentResponseDto
            {
                Id = s.Id,
                CustomerId = s.CustomerId,
                CustomerName = s.Customer?.FullName ?? s.Customer?.UserName ?? "Unknown",
                FromCity = s.FromCity,
                ToCity = s.ToCity,
                Weight = s.Weight,
                Description = s.Description,
                OfferedPrice = s.OfferedPrice,
                Status = s.Status,
                CreatedAt = s.CreatedAt
            }).ToList();
        }
    }
}

