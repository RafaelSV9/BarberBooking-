namespace BarberBooking.Api.DTOs;

public record CreateBlockedSlotRequest(
    Guid BarberId,
    DateTimeOffset StartsAt,
    string Reason
);
