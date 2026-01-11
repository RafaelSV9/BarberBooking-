namespace BarberBooking.Api.DTOs;

public record CreateAppointmentRequest(
    Guid BarberId,
    string CustomerName,
    string CustomerPhone,
    DateTimeOffset StartsAt
);
