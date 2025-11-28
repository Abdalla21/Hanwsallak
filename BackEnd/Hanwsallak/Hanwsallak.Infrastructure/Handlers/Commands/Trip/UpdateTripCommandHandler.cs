using Hanwsallak.Domain.Commands.Trip;
using Hanwsallak.Domain.DTO.Trip;
using Hanwsallak.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hanwsallak.Infrastructure.Handlers.Commands.Trip
{
    public class UpdateTripCommandHandler : IRequestHandler<UpdateTripCommand, TripResponseDto>
    {
        private readonly ApplicationDBContext _context;

        public UpdateTripCommandHandler(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<TripResponseDto> Handle(UpdateTripCommand request, CancellationToken cancellationToken)
        {
            var trip = await _context.Trips
                .Include(t => t.Traveler)
                .FirstOrDefaultAsync(t => t.Id == request.UpdateTripDto.Id && t.TravelerId == request.UserId, cancellationToken);

            if (trip == null)
                throw new KeyNotFoundException("Trip not found");

            trip.FromCity = request.UpdateTripDto.FromCity;
            trip.ToCity = request.UpdateTripDto.ToCity;
            trip.DepartureDate = request.UpdateTripDto.DepartureDate;
            trip.DepartureTime = request.UpdateTripDto.DepartureTime;
            trip.RecurringDay = request.UpdateTripDto.RecurringDay;
            trip.AvailableCapacity = request.UpdateTripDto.AvailableCapacity;
            trip.MaxPackages = request.UpdateTripDto.MaxPackages;
            trip.Status = request.UpdateTripDto.Status;

            await _context.SaveChangesAsync(cancellationToken);

            return new TripResponseDto
            {
                Id = trip.Id,
                TravelerId = trip.TravelerId,
                TravelerName = trip.Traveler?.FullName ?? trip.Traveler?.UserName ?? "Unknown",
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

