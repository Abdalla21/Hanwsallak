using Hanwsallak.Domain.DTO.Shipment;
using Hanwsallak.Domain.Queries.Shipment;
using Hanwsallak.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hanwsallak.Infrastructure.Handlers.Queries.Shipment
{
    public class GetAvailableShipmentsQueryHandler : IRequestHandler<GetAvailableShipmentsQuery, List<ShipmentResponseDto>>
    {
        private readonly ReadOnlyDBContext _context;

        public GetAvailableShipmentsQueryHandler(ReadOnlyDBContext context)
        {
            _context = context;
        }

        public async Task<List<ShipmentResponseDto>> Handle(GetAvailableShipmentsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Shipments
                .Include(s => s.Customer)
                .Where(s => s.Status == "Pending");

            if (!string.IsNullOrEmpty(request.FromCity))
                query = query.Where(s => s.FromCity.Contains(request.FromCity));

            if (!string.IsNullOrEmpty(request.ToCity))
                query = query.Where(s => s.ToCity.Contains(request.ToCity));

            var shipments = await query
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

