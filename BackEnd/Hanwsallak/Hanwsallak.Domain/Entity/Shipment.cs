using System.ComponentModel.DataAnnotations.Schema;

namespace Hanwsallak.Domain.Entity
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        
        [ForeignKey(nameof(CustomerId))]
        public ApplicationUser Customer { get; set; } = null!;
        
        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public decimal Weight { get; set; } // in kg
        public string Description { get; set; } = string.Empty;
        public decimal OfferedPrice { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Accepted, Delivered, Cancelled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

