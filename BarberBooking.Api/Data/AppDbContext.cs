using BarberBooking.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace BarberBooking.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<Barber> Barbers => Set<Barber>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<BlockedSlot> BlockedSlots => Set<BlockedSlot>();

    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>()
            .HasIndex(a => new { a.BarberId, a.StartsAt })
            .IsUnique();

        modelBuilder.Entity<BlockedSlot>()
            .HasIndex(b => new { b.BarberId, b.StartsAt })
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
