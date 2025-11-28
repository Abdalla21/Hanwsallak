using Hanwsallak.Domain.DTO.Trip;
using Hanwsallak.Domain.DTO.Shipment;

namespace Hanwsallak.Domain.DTO.Order
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public Guid ShipmentId { get; set; }
        public TripResponseDto Trip { get; set; } = null!;
        public ShipmentResponseDto Shipment { get; set; } = null!;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}

