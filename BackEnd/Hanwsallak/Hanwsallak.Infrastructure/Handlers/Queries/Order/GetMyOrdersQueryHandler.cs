using Hanwsallak.Domain.DTO.Order;
using Hanwsallak.Domain.DTO.Shipment;
using Hanwsallak.Domain.DTO.Trip;
using Hanwsallak.Domain.Queries.Order;
using Hanwsallak.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hanwsallak.Infrastructure.Handlers.Queries.Order
{
    public class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, List<OrderResponseDto>>
    {
        private readonly ReadOnlyDBContext _context;

        public GetMyOrdersQueryHandler(ReadOnlyDBContext context)
        {
            _context = context;
        }

        public async Task<List<OrderResponseDto>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                .Include(o => o.Trip)
                    .ThenInclude(t => t!.Traveler)
                .Include(o => o.Shipment)
                    .ThenInclude(s => s!.Customer)
                .Where(o => o.Trip!.TravelerId == request.UserId || o.Shipment!.CustomerId == request.UserId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(cancellationToken);

            return orders.Select(o => new OrderResponseDto
            {
                Id = o.Id,
                TripId = o.TripId,
                ShipmentId = o.ShipmentId,
                Trip = o.Trip != null ? new TripResponseDto
                {
                    Id = o.Trip.Id,
                    TravelerId = o.Trip.TravelerId,
                    TravelerName = o.Trip.Traveler?.FullName ?? o.Trip.Traveler?.UserName ?? "Unknown",
                    FromCity = o.Trip.FromCity,
                    ToCity = o.Trip.ToCity,
                    DepartureDate = o.Trip.DepartureDate,
                    DepartureTime = o.Trip.DepartureTime,
                    RecurringDay = o.Trip.RecurringDay,
                    AvailableCapacity = o.Trip.AvailableCapacity,
                    MaxPackages = o.Trip.MaxPackages,
                    Status = o.Trip.Status,
                    CreatedAt = o.Trip.CreatedAt
                } : null!,
                Shipment = o.Shipment != null ? new ShipmentResponseDto
                {
                    Id = o.Shipment.Id,
                    CustomerId = o.Shipment.CustomerId,
                    CustomerName = o.Shipment.Customer?.FullName ?? o.Shipment.Customer?.UserName ?? "Unknown",
                    FromCity = o.Shipment.FromCity,
                    ToCity = o.Shipment.ToCity,
                    Weight = o.Shipment.Weight,
                    Description = o.Shipment.Description,
                    OfferedPrice = o.Shipment.OfferedPrice,
                    Status = o.Shipment.Status,
                    CreatedAt = o.Shipment.CreatedAt
                } : null!,
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                DeliveredAt = o.DeliveredAt
            }).ToList();
        }
    }
}

