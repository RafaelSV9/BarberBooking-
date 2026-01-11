using BarberBooking.Api.Data;
using BarberBooking.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberBooking.Api.Controllers;

[ApiController]
[Route("api/barbers")]
public class BarbersController : ControllerBase
{
    private readonly AppDbContext _db;
    public BarbersController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BarberDto>>> GetActive()
    {
        var barbers = await _db.Barbers
            .Where(b => b.Active)
            .OrderBy(b => b.Name)
            .Select(b => new BarberDto(b.Id, b.Name))
            .ToListAsync();

        return Ok(barbers);
    }
}
