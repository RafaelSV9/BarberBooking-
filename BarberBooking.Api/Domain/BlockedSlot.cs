namespace BarberBooking.Api.Domain;

public class BlockedSlot
{
    public Guid Id { get; set; }
    public Guid BarberId { get; set; }
    public Barber? Barber { get; set; }

    public DateTimeOffset StartsAt { get; set; }
    public string Reason { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
