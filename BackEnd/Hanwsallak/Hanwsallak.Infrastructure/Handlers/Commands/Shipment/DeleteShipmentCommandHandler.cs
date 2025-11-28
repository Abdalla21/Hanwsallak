using Hanwsallak.Domain.Commands.Shipment;
using Hanwsallak.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hanwsallak.Infrastructure.Handlers.Commands.Shipment
{
    public class DeleteShipmentCommandHandler : IRequestHandler<DeleteShipmentCommand, bool>
    {
        private readonly ApplicationDBContext _context;

        public DeleteShipmentCommandHandler(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteShipmentCommand request, CancellationToken cancellationToken)
        {
            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.Id == request.ShipmentId && s.CustomerId == request.UserId, cancellationToken);

            if (shipment == null)
                return false;

            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

