using MySpot.Api.Exceptions;
using MySpot.Api.Models;

namespace MySpot.Api.Entities;

public class WeeklyParkingSpot
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public DateTime From { get; private set; }
    public DateTime To { get; private set; }
    public IEnumerable<Reservation> Reservations => _reservations;
    private readonly HashSet<Reservation> _reservations = new();

    public WeeklyParkingSpot(Guid id, DateTime from, DateTime to, string name)
    {
        Id = id;
        Name = name;
        From = from;
        To = to;
    }

    public void AddReservation(Reservation reservation)
    {
        var isInvalidDate = reservation.Date.Date < From || reservation.Date.Date > To ||
                            reservation.Date.Date < DateTime.UtcNow.Date;
        if (isInvalidDate)
        {
            throw new InvalidReservationDateException(reservation.Date);
        }

        var alreadyReserved = _reservations.Any(x => x.Date.Date == reservation.Date.Date);
        if (alreadyReserved)
        {
            throw new ParkingSpotAlreadyReservedException(Name, reservation.Date);
        }

        _reservations.Add(reservation);
    }

    public void RemoveReservation(Guid id)
    {
        _reservations.RemoveWhere(x => x.Id == id);
    }
}