namespace Hanwsallak.Domain.DTO.Trip
{
    public class CreateTripDto
    {
        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DayOfWeek? RecurringDay { get; set; }
        public decimal AvailableCapacity { get; set; }
        public int MaxPackages { get; set; }
    }
}

