using Hanwsallak.Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hanwsallak.Infrastructure.Data
{
    public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : IdentityDbContext<ApplicationUser, ApplicationRoles, Guid>(options)
    {
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Order> Orders { get; set; }

        override protected void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Trip entity
            builder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Traveler)
                    .WithMany()
                    .HasForeignKey(e => e.TravelerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Shipment entity
            builder.Entity<Shipment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Customer)
                    .WithMany()
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Order entity
            builder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Trip)
                    .WithMany()
                    .HasForeignKey(e => e.TripId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Shipment)
                    .WithMany()
                    .HasForeignKey(e => e.ShipmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
