using BarberBooking.Api.Data;
using BarberBooking.Api.Domain;
using BarberBooking.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberBooking.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/admin/blocked-slots")]
public class AdminBlockedSlotsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AdminBlockedSlotsController(AppDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBlockedSlotRequest req)
    {
        var barber = await _db.Barbers.FirstOrDefaultAsync(b => b.Id == req.BarberId);
        if (barber is null) return BadRequest("invalid barberId");

        var existsAppt = await _db.Appointments.AnyAsync(a => a.BarberId == req.BarberId && a.StartsAt == req.StartsAt && a.Status != AppointmentStatus.CANCELADO);
        if (existsAppt) return Conflict("slot already booked");

        var existsBlock = await _db.BlockedSlots.AnyAsync(b => b.BarberId == req.BarberId && b.StartsAt == req.StartsAt);
        if (existsBlock) return Conflict("slot already blocked");

        var block = new BlockedSlot
        {
            Id = Guid.NewGuid(),
            BarberId = req.BarberId,
            StartsAt = req.StartsAt,
            Reason = req.Reason ?? "",
            CreatedAt = DateTimeOffset.UtcNow
        };

        _db.BlockedSlots.Add(block);
        await _db.SaveChangesAsync();
        return Created("", new { block.Id });
    }
}
