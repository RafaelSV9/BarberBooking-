using BarberBooking.Api.Data;
using BarberBooking.Api.DTOs;
using BarberBooking.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberBooking.Api.Controllers;

[ApiController]
[Route("api/availability")]
public class AvailabilityController : ControllerBase
{
    private readonly SlotService _slots;

    public AvailabilityController(SlotService slots) => _slots = slots;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SlotDto>>> Get([FromQuery] Guid barberId, [FromQuery] string date)
    {
        if (!DateOnly.TryParse(date, out var d))
            return BadRequest("date must be YYYY-MM-DD");

        var unavailable = await _slots.GetUnavailableSlots(barberId, d);
        var all = _slots.GenerateSlotsForDate(d);

        var result = all.Select(s =>
        {
            var local = TimeZoneInfo.ConvertTime(s, TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"));
            var label = local.ToString("HH:mm");
            var available = !unavailable.Contains(s);
            return new SlotDto(label, available);
        });

        return Ok(result);
    }
}
