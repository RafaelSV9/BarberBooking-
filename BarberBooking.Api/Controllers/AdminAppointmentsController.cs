using BarberBooking.Api.Data;
using BarberBooking.Api.Domain;
using BarberBooking.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberBooking.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/admin/appointments")]
public class AdminAppointmentsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AdminAppointmentsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> List([FromQuery] string date, [FromQuery] Guid? barberId)
    {
        if (!DateOnly.TryParse(date, out var d))
            return BadRequest("date must be YYYY-MM-DD");

        var dayStart = new DateTimeOffset(d.ToDateTime(new TimeSpan(0,0,0)), TimeSpan.FromHours(-3));
        var dayEnd = new DateTimeOffset(d.ToDateTime(new TimeSpan(23,59,59)), TimeSpan.FromHours(-3));

        var q = _db.Appointments
            .Include(a => a.Barber)
            .Where(a => a.StartsAt >= dayStart && a.StartsAt <= dayEnd);

        if (barberId.HasValue) q = q.Where(a => a.BarberId == barberId.Value);

        var items = await q.OrderBy(a => a.StartsAt).ToListAsync();

        var dtos = items.Select(a => new AppointmentDto(
            a.Id,
            a.BarberId,
            a.Barber?.Name ?? "",
            a.CustomerName,
            a.CustomerPhone,
            a.StartsAt,
            a.Status
        ));

        return Ok(dtos);
    }

    [HttpPatch("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid id)
    {
        var appt = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == id);
        if (appt is null) return NotFound();

        appt.Status = AppointmentStatus.CONFIRMADO;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var appt = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == id);
        if (appt is null) return NotFound();

        appt.Status = AppointmentStatus.CANCELADO;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
