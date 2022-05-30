using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;
using MySpot.Api.Services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController : ControllerBase
{

    private readonly ReservationService _service = new();

    [HttpGet]
    public ActionResult<Reservation[]> Get()
    {
        return Ok(_service.GetAllWeekly());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Reservation> Get(int id)
    {
        var reservation = _service.Get(id);
        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post(Reservation reservation)
    {
        var id = _service.Create(reservation);
        if (id is null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Get), new {Id = reservation.Id}, default);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Reservation> Put(int id, [FromBody] Reservation reservation)
    {
        reservation.Id = id;
        var succeeded = _service.Update(reservation);
        if (!succeeded)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var succeeded = _service.Delete(id);
        if (!succeeded)
        {
            return BadRequest();
        }
        return NoContent();
    }
}