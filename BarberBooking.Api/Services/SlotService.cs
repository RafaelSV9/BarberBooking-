using BarberBooking.Api.Data;
using BarberBooking.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace BarberBooking.Api.Services;

public class SlotService
{
    private readonly AppDbContext _db;
    private readonly TimeZoneInfo _tz;

    public SlotService(AppDbContext db, string timezoneId)
    {
        _db = db;
        _tz = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
    }

    public static bool IsValidHalfHour(DateTimeOffset startsAt, TimeZoneInfo tz)
    {
        // convert to local time for validation
        var local = TimeZoneInfo.ConvertTime(startsAt, tz);
        return local.Minute is 0 or 30 && local.Second == 0;
    }

    public static bool IsWithinBusinessHours(DateTimeOffset startsAt, TimeZoneInfo tz)
    {
        var local = TimeZoneInfo.ConvertTime(startsAt, tz);
        var t = local.TimeOfDay;
        // Slots from 09:00 to 18:30 inclusive
        var open = new TimeSpan(9, 0, 0);
        var lastStart = new TimeSpan(18, 30, 0);
        return t >= open && t <= lastStart;
    }

    public static bool IsWithin30Days(DateTimeOffset startsAt, TimeZoneInfo tz)
    {
        var nowLocal = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tz).Date;
        var startLocal = TimeZoneInfo.ConvertTime(startsAt, tz).Date;
        return startLocal >= nowLocal && startLocal <= nowLocal.AddDays(30);
    }

    public IEnumerable<DateTimeOffset> GenerateSlotsForDate(DateOnly date)
    {
        // generate local slots and convert to offset in tz
        var open = new TimeSpan(9, 0, 0);
        var lastStart = new TimeSpan(18, 30, 0);

        var localDateTime = date.ToDateTime(open);
        var endLocal = date.ToDateTime(lastStart);

        while (localDateTime <= endLocal)
        {
            var unspecified = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);
            var offset = new DateTimeOffset(unspecified, _tz.GetUtcOffset(unspecified));
            yield return offset;

            localDateTime = localDateTime.AddMinutes(30);
        }
    }

    public async Task<HashSet<DateTimeOffset>> GetUnavailableSlots(Guid barberId, DateOnly date)
    {
        var dayStartLocal = date.ToDateTime(new TimeSpan(0, 0, 0));
        var dayEndLocal = date.ToDateTime(new TimeSpan(23, 59, 59));

        var start = new DateTimeOffset(DateTime.SpecifyKind(dayStartLocal, DateTimeKind.Unspecified), _tz.GetUtcOffset(dayStartLocal));
        var end = new DateTimeOffset(DateTime.SpecifyKind(dayEndLocal, DateTimeKind.Unspecified), _tz.GetUtcOffset(dayEndLocal));

        var busyAppointments = await _db.Appointments
            .Where(a => a.BarberId == barberId &&
                        a.StartsAt >= start &&
                        a.StartsAt <= end &&
                        a.Status != AppointmentStatus.CANCELADO)
            .Select(a => a.StartsAt)
            .ToListAsync();

        var blocked = await _db.BlockedSlots
            .Where(b => b.BarberId == barberId &&
                        b.StartsAt >= start &&
                        b.StartsAt <= end)
            .Select(b => b.StartsAt)
            .ToListAsync();

        return new HashSet<DateTimeOffset>(busyAppointments.Concat(blocked));
    }
}
