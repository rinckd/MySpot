﻿using MySpot.Api.Exceptions;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Entities;

public class WeeklyParkingSpot
{
    public ParkingSpotId Id { get; }
    public ParkingSpotName Name { get; private set; }

    public Week Week { get; private set; }
    public IEnumerable<Reservation> Reservations => _reservations;
    private readonly HashSet<Reservation> _reservations = new();

    public WeeklyParkingSpot(ParkingSpotId id, Week week, ParkingSpotName name)
    {
        Id = id;
        Name = name;
        Week = week;
    }

    public void AddReservation(Reservation reservation, Date now)
    {
        var isInvalidDate = reservation.Date < Week.From || 
                            reservation.Date > Week.To ||
                            reservation.Date < now;
        if (isInvalidDate)
        {
            throw new InvalidReservationDateException(reservation.Date.Value.Date);
        }

        var alreadyReserved = _reservations.Any(x => x.Date == reservation.Date);
        if (alreadyReserved)
        {
            throw new ParkingSpotAlreadyReservedException(Name, reservation.Date.Value.Date);
        }

        _reservations.Add(reservation);
    }

    public void RemoveReservation(ReservationId id)
    {
        _reservations.RemoveWhere(x => x.Id == id);
    }
}