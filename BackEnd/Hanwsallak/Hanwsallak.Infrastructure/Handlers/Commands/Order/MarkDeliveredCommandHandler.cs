using Hanwsallak.Domain.Commands.Order;
using Hanwsallak.Domain.DTO.Order;
using Hanwsallak.Domain.DTO.Shipment;
using Hanwsallak.Domain.DTO.Trip;
using Hanwsallak.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hanwsallak.Infrastructure.Handlers.Commands.Order
{
    public class MarkDeliveredCommandHandler : IRequestHandler<MarkDeliveredCommand, OrderResponseDto>
    {
        private readonly ApplicationDBContext _context;

        public MarkDeliveredCommandHandler(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<OrderResponseDto> Handle(MarkDeliveredCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.Trip)
                    .ThenInclude(t => t!.Traveler)
                .Include(o => o.Shipment)
                    .ThenInclude(s => s!.Customer)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null)
                throw new KeyNotFoundException("Order not found");

            if (order.Status != "InTransit")
                throw new InvalidOperationException("Order must be in transit before marking as delivered");

            if (order.Trip?.TravelerId != request.UserId)
                throw new UnauthorizedAccessException("Only the traveler can mark order as delivered");

            order.Status = "Delivered";
            order.DeliveredAt = DateTime.UtcNow;
            if (order.Shipment != null)
                order.Shipment.Status = "Delivered";

            await _context.SaveChangesAsync(cancellationToken);

            return new OrderResponseDto
            {
                Id = order.Id,
                TripId = order.TripId,
                ShipmentId = order.ShipmentId,
                Trip = order.Trip != null ? new TripResponseDto
                {
                    Id = order.Trip.Id,
                    TravelerId = order.Trip.TravelerId,
                    TravelerName = order.Trip.Traveler?.FullName ?? order.Trip.Traveler?.UserName ?? "Unknown",
                    FromCity = order.Trip.FromCity,
                    ToCity = order.Trip.ToCity,
                    DepartureDate = order.Trip.DepartureDate,
                    DepartureTime = order.Trip.DepartureTime,
                    RecurringDay = order.Trip.RecurringDay,
                    AvailableCapacity = order.Trip.AvailableCapacity,
                    MaxPackages = order.Trip.MaxPackages,
                    Status = order.Trip.Status,
                    CreatedAt = order.Trip.CreatedAt
                } : null!,
                Shipment = order.Shipment != null ? new ShipmentResponseDto
                {
                    Id = order.Shipment.Id,
                    CustomerId = order.Shipment.CustomerId,
                    CustomerName = order.Shipment.Customer?.FullName ?? order.Shipment.Customer?.UserName ?? "Unknown",
                    FromCity = order.Shipment.FromCity,
                    ToCity = order.Shipment.ToCity,
                    Weight = order.Shipment.Weight,
                    Description = order.Shipment.Description,
                    OfferedPrice = order.Shipment.OfferedPrice,
                    Status = order.Shipment.Status,
                    CreatedAt = order.Shipment.CreatedAt
                } : null!,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                DeliveredAt = order.DeliveredAt
            };
        }
    }
}

