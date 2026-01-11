namespace BarberBooking.Api.DTOs;

public record AdminLoginRequest(string User, string Password);
public record AdminLoginResponse(string Token);
