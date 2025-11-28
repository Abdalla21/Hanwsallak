namespace Hanwsallak.Domain.DTO.Trip
{
    public class TripResponseDto
    {
        public Guid Id { get; set; }
        public Guid TravelerId { get; set; }
        public string TravelerName { get; set; } = string.Empty;
        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DayOfWeek? RecurringDay { get; set; }
        public decimal AvailableCapacity { get; set; }
        public int MaxPackages { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

