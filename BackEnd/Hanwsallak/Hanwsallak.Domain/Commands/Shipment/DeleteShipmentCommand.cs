using MediatR;

namespace Hanwsallak.Domain.Commands.Shipment
{
    public class DeleteShipmentCommand : IRequest<bool>
    {
        public Guid ShipmentId { get; set; }
        public Guid UserId { get; set; }
    }
}

