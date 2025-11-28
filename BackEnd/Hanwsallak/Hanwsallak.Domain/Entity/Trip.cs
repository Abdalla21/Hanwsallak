using System.ComponentModel.DataAnnotations.Schema;

namespace Hanwsallak.Domain.Entity
{
    public class Trip
    {
        public Guid Id { get; set; }
        public Guid TravelerId { get; set; }
        
        [ForeignKey(nameof(TravelerId))]
        public ApplicationUser Traveler { get; set; } = null!;
        
        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DayOfWeek? RecurringDay { get; set; } // For weekly recurring trips
        public decimal AvailableCapacity { get; set; } // in kg
        public int MaxPackages { get; set; }
        public string Status { get; set; } = "Available"; // Available, Completed, Cancelled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

