using MySpot.Api.Entities;
using MySpot.Api.Models;

namespace MySpot.Api.Services;

public sealed class ReservationService
{
    private static WeeklyParkingSpot[] _weeklyParkingSpots =
    {
        new WeeklyParkingSpot(Guid.NewGuid(), DateTime.UtcNow.AddDays(-5), DateTime.UtcNow.AddDays(2), "P1"),
        new WeeklyParkingSpot(Guid.NewGuid(), DateTime.UtcNow.AddDays(-5), DateTime.UtcNow.AddDays(2), "P2"),
        new WeeklyParkingSpot(Guid.NewGuid(), DateTime.UtcNow.AddDays(-5), DateTime.UtcNow.AddDays(2), "P3"),
        new WeeklyParkingSpot(Guid.NewGuid(), DateTime.UtcNow.AddDays(-5), DateTime.UtcNow.AddDays(2), "P4"),
        new WeeklyParkingSpot(Guid.NewGuid(), DateTime.UtcNow.AddDays(-5), DateTime.UtcNow.AddDays(2), "P5"),
    };
    
    public IEnumerable<Reservation> GetAllWeekly() => _weeklyParkingSpots.SelectMany(x => x.Reservations);
    public Reservation? Get(Guid id) => GetAllWeekly().SingleOrDefault(x => x.Id == id);

    public int? Create(Guid parkingSpotId, Reservation reservation)
    {
        var weeklyParkingSpot = _weeklyParkingSpots.SingleOrDefault(x => x.Id == parkingSpotId);
        if (weeklyParkingSpot is null)
        {
            return default;
        }

        // reservation.Id = Guid.NewGuid();
        weeklyParkingSpot.AddReservation(reservation);
    }

    public bool Update(Reservation reservation)
    {
        var existingReservation = _reservations.SingleOrDefault(x => x.Id == reservation.Id);
        if (existingReservation is null)
        {
            return false;
        }

        existingReservation.LicensePlate = reservation.LicensePlate;
        return true;
    }
    
    public bool Delete(int id)
    {
        var existingReservation = _reservations.SingleOrDefault(x => x.Id == id);
        if (existingReservation is null)
        {
            return false;
        }

        return _reservations.Remove(existingReservation);
 
    }
}