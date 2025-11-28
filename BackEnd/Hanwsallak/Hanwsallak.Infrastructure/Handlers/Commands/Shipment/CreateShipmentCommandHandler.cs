using Hanwsallak.Domain.Commands.Shipment;
using Hanwsallak.Domain.DTO.Shipment;
using Hanwsallak.Domain.Entity;
using Hanwsallak.Infrastructure.Data;
using MediatR;

namespace Hanwsallak.Infrastructure.Handlers.Commands.Shipment
{
    public class CreateShipmentCommandHandler : IRequestHandler<CreateShipmentCommand, ShipmentResponseDto>
    {
        private readonly ApplicationDBContext _context;

        public CreateShipmentCommandHandler(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ShipmentResponseDto> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
        {
            var shipment = new Hanwsallak.Domain.Entity.Shipment
            {
                Id = Guid.NewGuid(),
                CustomerId = request.UserId,
                FromCity = request.CreateShipmentDto.FromCity,
                ToCity = request.CreateShipmentDto.ToCity,
                Weight = request.CreateShipmentDto.Weight,
                Description = request.CreateShipmentDto.Description,
                OfferedPrice = request.CreateShipmentDto.OfferedPrice,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Shipments.Add(shipment);
            await _context.SaveChangesAsync(cancellationToken);

            var customer = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);

            return new ShipmentResponseDto
            {
                Id = shipment.Id,
                CustomerId = shipment.CustomerId,
                CustomerName = customer?.FullName ?? customer?.UserName ?? "Unknown",
                FromCity = shipment.FromCity,
                ToCity = shipment.ToCity,
                Weight = shipment.Weight,
                Description = shipment.Description,
                OfferedPrice = shipment.OfferedPrice,
                Status = shipment.Status,
                CreatedAt = shipment.CreatedAt
            };
        }
    }
}

