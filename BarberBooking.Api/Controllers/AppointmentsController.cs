using BarberBooking.Api.Data;
using BarberBooking.Api.Domain;
using BarberBooking.Api.DTOs;
using BarberBooking.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberBooking.Api.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly SlotService _slots;
    private readonly TimeZoneInfo _tz;

    public AppointmentsController(AppDbContext db, SlotService slots, IConfiguration cfg)
    {
        _db = db;
        _slots = slots;
        _tz = TimeZoneInfo.FindSystemTimeZoneById(cfg["App:Timezone"] ?? "America/Sao_Paulo");
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> Create([FromBody] CreateAppointmentRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.CustomerName) || req.CustomerName.Length < 2)
            return BadRequest("customerName is required");
        if (string.IsNullOrWhiteSpace(req.CustomerPhone) || req.CustomerPhone.Length < 8)
            return BadRequest("customerPhone is required");

        if (!SlotService.IsWithin30Days(req.StartsAt, _tz))
            return BadRequest("startsAt must be within the next 30 days");

        if (!SlotService.IsValidHalfHour(req.StartsAt, _tz))
            return BadRequest("startsAt must be on a 30-minute slot");

        if (!SlotService.IsWithinBusinessHours(req.StartsAt, _tz))
            return BadRequest("startsAt must be within business hours (09:00-18:30)");

        var barber = await _db.Barbers.FirstOrDefaultAsync(b => b.Id == req.BarberId && b.Active);
        if (barber is null) return BadRequest("invalid barberId");

        // check blocked
        var isBlocked = await _db.BlockedSlots.AnyAsync(b => b.BarberId == req.BarberId && b.StartsAt == req.StartsAt);
        if (isBlocked) return Conflict("slot is blocked");

        // check conflicts
        var exists = await _db.Appointments.AnyAsync(a => a.BarberId == req.BarberId && a.StartsAt == req.StartsAt && a.Status != AppointmentStatus.CANCELADO);
        if (exists) return Conflict("slot not available");

        var appt = new Appointment
        {
            Id = Guid.NewGuid(),
            BarberId = req.BarberId,
            CustomerName = req.CustomerName.Trim(),
            CustomerPhone = req.CustomerPhone.Trim(),
            StartsAt = req.StartsAt,
            Status = AppointmentStatus.PENDENTE,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _db.Appointments.Add(appt);
        await _db.SaveChangesAsync();

        var dto = new AppointmentDto(appt.Id, barber.Id, barber.Name, appt.CustomerName, appt.CustomerPhone, appt.StartsAt, appt.Status);
        return CreatedAtAction(nameof(GetById), new { id = appt.Id }, dto);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AppointmentDto>> GetById(Guid id)
    {
        var appt = await _db.Appointments
            .Include(a => a.Barber)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (appt is null) return NotFound();

        var dto = new AppointmentDto(
            appt.Id,
            appt.BarberId,
            appt.Barber?.Name ?? "",
            appt.CustomerName,
            appt.CustomerPhone,
            appt.StartsAt,
            appt.Status
        );

        return Ok(dto);
    }
}
