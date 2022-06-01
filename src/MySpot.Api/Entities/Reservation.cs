using MySpot.Api.ValueObjects;

namespace MySpot.Api.Entities;

public class Reservation
{
    public ReservationId Id { get; }
    public EmployeeName EmployeeName { get; private set; }
    public LicensePlate LicensePlate { get; private set; }

    public Date Date { get; private set; }

    public Reservation(ReservationId id, EmployeeName employeeName, LicensePlate licensePlate, Date date)
    {
        Id = id;
        EmployeeName = employeeName;
        LicensePlate = licensePlate;
        Date = date;
    }

    public void ChangeLicensePlate(LicensePlate licensePlate) => LicensePlate = licensePlate;
}