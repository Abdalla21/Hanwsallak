using Hanwsallak.Domain.DTO.Shipment;
using MediatR;

namespace Hanwsallak.Domain.Commands.Shipment
{
    public class CreateShipmentCommand : IRequest<ShipmentResponseDto>
    {
        public CreateShipmentDto CreateShipmentDto { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}

