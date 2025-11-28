using Hanwsallak.Domain.Commands.Order;
using Hanwsallak.Domain.DTO.Order;
using Hanwsallak.Domain.DTO.Shipment;
using Hanwsallak.Domain.DTO.Trip;
using Hanwsallak.Domain.Entity;
using Hanwsallak.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hanwsallak.Infrastructure.Handlers.Commands.Order
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponseDto>
    {
        private readonly ApplicationDBContext _context;

        public CreateOrderCommandHandler(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<OrderResponseDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var trip = await _context.Trips
                .Include(t => t.Traveler)
                .FirstOrDefaultAsync(t => t.Id == request.CreateOrderDto.TripId, cancellationToken);

            var shipment = await _context.Shipments
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(s => s.Id == request.CreateOrderDto.ShipmentId, cancellationToken);

            if (trip == null || shipment == null)
                throw new KeyNotFoundException("Trip or Shipment not found");

            if (trip.TravelerId != request.UserId && shipment.CustomerId != request.UserId)
                throw new UnauthorizedAccessException("User is not authorized to create this order");

            if (shipment.Status != "Pending")
                throw new InvalidOperationException("Shipment is not available");

            if (trip.Status != "Available")
                throw new InvalidOperationException("Trip is not available");

            if (shipment.Weight > trip.AvailableCapacity)
                throw new InvalidOperationException("Shipment weight exceeds trip capacity");

            var order = new Hanwsallak.Domain.Entity.Order
            {
                Id = Guid.NewGuid(),
                TripId = trip.Id,
                ShipmentId = shipment.Id,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return new OrderResponseDto
            {
                Id = order.Id,
                TripId = order.TripId,
                ShipmentId = order.ShipmentId,
                Trip = new TripResponseDto
                {
                    Id = trip.Id,
                    TravelerId = trip.TravelerId,
                    TravelerName = trip.Traveler?.FullName ?? trip.Traveler?.UserName ?? "Unknown",
                    FromCity = trip.FromCity,
                    ToCity = trip.ToCity,
                    DepartureDate = trip.DepartureDate,
                    DepartureTime = trip.DepartureTime,
                    RecurringDay = trip.RecurringDay,
                    AvailableCapacity = trip.AvailableCapacity,
                    MaxPackages = trip.MaxPackages,
                    Status = trip.Status,
                    CreatedAt = trip.CreatedAt
                },
                Shipment = new ShipmentResponseDto
                {
                    Id = shipment.Id,
                    CustomerId = shipment.CustomerId,
                    CustomerName = shipment.Customer?.FullName ?? shipment.Customer?.UserName ?? "Unknown",
                    FromCity = shipment.FromCity,
                    ToCity = shipment.ToCity,
                    Weight = shipment.Weight,
                    Description = shipment.Description,
                    OfferedPrice = shipment.OfferedPrice,
                    Status = shipment.Status,
                    CreatedAt = shipment.CreatedAt
                },
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                DeliveredAt = order.DeliveredAt
            };
        }
    }
}

