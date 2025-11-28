using Hanwsallak.Domain.Commands.Trip;
using Hanwsallak.Domain.DTO.Trip;
using Hanwsallak.Domain.Entity;
using Hanwsallak.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hanwsallak.Infrastructure.Handlers.Commands.Trip
{
    public class CreateTripCommandHandler : IRequestHandler<CreateTripCommand, TripResponseDto>
    {
        private readonly ApplicationDBContext _context;

        public CreateTripCommandHandler(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<TripResponseDto> Handle(CreateTripCommand request, CancellationToken cancellationToken)
        {
            var trip = new Hanwsallak.Domain.Entity.Trip
            {
                Id = Guid.NewGuid(),
                TravelerId = request.UserId,
                FromCity = request.CreateTripDto.FromCity,
                ToCity = request.CreateTripDto.ToCity,
                DepartureDate = request.CreateTripDto.DepartureDate,
                DepartureTime = request.CreateTripDto.DepartureTime,
                RecurringDay = request.CreateTripDto.RecurringDay,
                AvailableCapacity = request.CreateTripDto.AvailableCapacity,
                MaxPackages = request.CreateTripDto.MaxPackages,
                Status = "Available",
                CreatedAt = DateTime.UtcNow
            };

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync(cancellationToken);

            var traveler = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);

            return new TripResponseDto
            {
                Id = trip.Id,
                TravelerId = trip.TravelerId,
                TravelerName = traveler?.FullName ?? traveler?.UserName ?? "Unknown",
                FromCity = trip.FromCity,
                ToCity = trip.ToCity,
                DepartureDate = trip.DepartureDate,
                DepartureTime = trip.DepartureTime,
                RecurringDay = trip.RecurringDay,
                AvailableCapacity = trip.AvailableCapacity,
                MaxPackages = trip.MaxPackages,
                Status = trip.Status,
                CreatedAt = trip.CreatedAt
            };
        }
    }
}

