using BarberBooking.Api.Domain;

namespace BarberBooking.Api.DTOs;

public record AppointmentDto(
    Guid Id,
    Guid BarberId,
    string BarberName,
    string CustomerName,
    string CustomerPhone,
    DateTimeOffset StartsAt,
    AppointmentStatus Status
);
