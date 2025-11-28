namespace Hanwsallak.Domain.DTO.Shipment
{
    public class CreateShipmentDto
    {
        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal OfferedPrice { get; set; }
    }
}

