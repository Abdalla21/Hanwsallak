namespace Hanwsallak.Domain.DTO.Shipment
{
    public class ShipmentResponseDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal OfferedPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

