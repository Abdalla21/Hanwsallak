using Hanwsallak.Domain.DTO.Shipment;
using Hanwsallak.Domain.Queries.Shipment;
using Hanwsallak.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hanwsallak.Infrastructure.Handlers.Queries.Shipment
{
    public class GetMyShipmentsQueryHandler : IRequestHandler<GetMyShipmentsQuery, List<ShipmentResponseDto>>
    {
        private readonly ReadOnlyDBContext _context;

        public GetMyShipmentsQueryHandler(ReadOnlyDBContext context)
        {
            _context = context;
        }

        public async Task<List<ShipmentResponseDto>> Handle(GetMyShipmentsQuery request, CancellationToken cancellationToken)
        {
            var shipments = await _context.Shipments
                .Include(s => s.Customer)
                .Where(s => s.CustomerId == request.UserId)
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

