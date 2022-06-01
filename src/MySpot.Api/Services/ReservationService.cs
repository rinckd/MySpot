﻿using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.Exceptions;
using MySpot.Api.ValueObjects;


namespace MySpot.Api.Services;

public sealed class ReservationService
{
    private static WeeklyParkingSpot[] _weeklyParkingSpots =
    {
        new(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(CurrentDate()),
            "P1"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(CurrentDate()),
            "P2"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(CurrentDate()),
            "P3"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(CurrentDate()),
            "P4"),
        new(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(CurrentDate()),
            "P5"),
    };

    public IEnumerable<ReservationDto> GetAllWeekly() => _weeklyParkingSpots
        .SelectMany(x => x.Reservations)
        .Select(x =>
            new ReservationDto
            {
                Id = x.Id,
                EmployeeName = x.EmployeeName,
                Date = x.Date.Value.Date
            });

    public ReservationDto? Get(Guid id) => GetAllWeekly().SingleOrDefault(x => x.Id == id);

    public Guid? Create(CreateReservation command)
    {
        try
        {
            var (spotId, reservationId, employeeName, licensePlate, date) = command;

            var parkingSpotId = new ParkingSpotId(spotId);
            var weeklyParkingSpot = _weeklyParkingSpots.SingleOrDefault(x => x.Id == parkingSpotId);
            if (weeklyParkingSpot is null)
            {
                return default;
            }

            var reservation = new Reservation(reservationId, employeeName, licensePlate, new Date(date));

            weeklyParkingSpot.AddReservation(reservation, new Date(CurrentDate()));
            return reservation.Id;

        }
        catch (CustomException e)
        {
            return default;
        }
    }

    public bool Update(ChangeReservationLicensePlate command)
    {
        try
        {
            var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
            if (weeklyParkingSpot is null)
            {
                return false;
            }

            var reservationId = new ReservationId(command.ReservationId);
            var reservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id == reservationId);
            if (reservation is null)
            {
                return false;
            }

            reservation.ChangeLicensePlate(command.LicensePlate);
            return true;
        }
        catch (CustomException e)
        {
            return false;
        }

    }

    public bool Delete(DeleteReservation command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
        if (weeklyParkingSpot is null)
        {
            return false;
        }

        weeklyParkingSpot.RemoveReservation(command.ReservationId);
        return true;
    }

    private WeeklyParkingSpot? GetWeeklyParkingSpotByReservation(ReservationId id) =>
        _weeklyParkingSpots.SingleOrDefault(x => x.Reservations.Any(r => r.Id == id));

    private static DateTime CurrentDate() => new Clock().Current();
}