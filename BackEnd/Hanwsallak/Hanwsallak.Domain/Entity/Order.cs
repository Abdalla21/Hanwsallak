using System.ComponentModel.DataAnnotations.Schema;

namespace Hanwsallak.Domain.Entity
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public Guid ShipmentId { get; set; }
        
        [ForeignKey(nameof(TripId))]
        public Trip Trip { get; set; } = null!;
        
        [ForeignKey(nameof(ShipmentId))]
        public Shipment Shipment { get; set; } = null!;
        
        public string Status { get; set; } = "Pending"; // Pending, Accepted, InTransit, Delivered, Cancelled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveredAt { get; set; }
    }
}

