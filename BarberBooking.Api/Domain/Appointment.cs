namespace BarberBooking.Api.Domain;

public enum AppointmentStatus
{
    PENDENTE = 0,
    CONFIRMADO = 1,
    CANCELADO = 2
}

public class Appointment
{
    public Guid Id { get; set; }
    public Guid BarberId { get; set; }
    public Barber? Barber { get; set; }

    public string CustomerName { get; set; } = "";
    public string CustomerPhone { get; set; } = "";
    public DateTimeOffset StartsAt { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.PENDENTE;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
