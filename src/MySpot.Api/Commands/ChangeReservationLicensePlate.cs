namespace MySpot.Api.Commands;

public sealed record ChangeReservationLicensePlate(Guid ReservationId, string LicensePlate);
