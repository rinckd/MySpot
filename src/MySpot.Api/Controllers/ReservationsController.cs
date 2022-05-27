using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController : ControllerBase
{
    private static string[] _parkingSpotNames = {"P1", "P2", "P3", "P4", "P5"};
    private static readonly List<Reservation> _reservations = new();

    private static int Id = 1;

    [HttpGet]
    public ActionResult<Reservation[]> Get()
    {
        return Ok(_reservations);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Reservation> Get(int id)
    {
        var reservation = _reservations.SingleOrDefault(x => x.Id == id);
        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post(Reservation reservation)
    {   
        reservation.Id = Id;
        reservation.Date = DateTime.Now.AddDays(1).Date;



        if (_parkingSpotNames.All(x => x != reservation.ParkingSpotName))
        {
            return BadRequest();
        }

        var reservationAlreadyExists = _reservations.Any(x =>
            x.Date.Date == reservation.Date.Date && x.ParkingSpotName == reservation.ParkingSpotName);
        if (reservationAlreadyExists)
        {
 
            return BadRequest();
        }
        
        Id++;
        _reservations.Add(reservation);
        return CreatedAtAction(nameof(Get), new {Id = reservation.Id}, default);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Reservation> Put(int id, [FromBody] Reservation reservation)
    {
        var existingReservation = _reservations.SingleOrDefault(x => x.Id == id);
        if (existingReservation == null)
        {
            return BadRequest();
        }

        existingReservation.LicensePlate = reservation.LicensePlate;
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var existingReservation = _reservations.SingleOrDefault(x => x.Id == id);
        if (existingReservation == null)
        {
            return BadRequest();
        }

        _reservations.Remove(existingReservation);
        return NoContent();
    }
}