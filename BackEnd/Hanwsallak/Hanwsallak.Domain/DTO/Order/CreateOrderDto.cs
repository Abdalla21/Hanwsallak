namespace Hanwsallak.Domain.DTO.Order
{
    public class CreateOrderDto
    {
        public Guid TripId { get; set; }
        public Guid ShipmentId { get; set; }
    }
}

