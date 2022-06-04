using MySpot.Api.Commands;
using MySpot.Api.Entities;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;
using MySpot.Tests.Unit.Shared;
using Shouldly;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
    [Fact]
    public void given_valid_command_should_add_reservation()
    {
        // Arrange
        var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.NewGuid(),
            "Joe Doe", "XYZ123", DateTime.UtcNow.AddDays(1));
        var reservationId = _reservationsService.Create(command);
        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe(command.ReservationId);
    }

    [Fact]
    public void given_invalid_parking_spot_id_command_should_not_add_reservation()
    {
        // Arrange
        var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0010-000000000001"), Guid.NewGuid(),
            "Joe Doe", "XYZ123", DateTime.UtcNow.AddDays(1));
        var reservationId = _reservationsService.Create(command);
        reservationId.ShouldBeNull();
    }

    [Fact]
    public void given_reservation_for_already_taken_date_create_should_fail()
    {
        // Arrange
        var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.NewGuid(),
            "Joe Doe", "XYZ123", DateTime.UtcNow.AddDays(1));
        _reservationsService.Create(command);
        var reservationId = _reservationsService.Create(command);
        reservationId.ShouldBeNull();
    }

    private readonly IClock _clock;
    private readonly ReservationService _reservationsService;

    public ReservationServiceTests()
    {
        _clock = new TestClock();
        var weeklyParkingSpots = new List<WeeklyParkingSpot>
        {
            new(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(_clock.Current()),
                "P1"),
            new(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(_clock.Current()),
                "P2"),
            new(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(_clock.Current()),
                "P3"),
            new(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(_clock.Current()),
                "P4"),
            new(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(_clock.Current()),
                "P5"),
        };
        _reservationsService = new ReservationService(_clock, weeklyParkingSpots);
    }
}