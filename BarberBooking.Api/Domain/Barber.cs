namespace BarberBooking.Api.Domain;

public class Barber
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public bool Active { get; set; } = true;
}
