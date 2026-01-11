using System.Text;
using BarberBooking.Api.Data;
using BarberBooking.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Load .env if present (simple)
var envPath = Path.Combine(builder.Environment.ContentRootPath, ".env");
if (File.Exists(envPath))
{
    foreach (var line in File.ReadAllLines(envPath))
    {
        if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#")) continue;
        var idx = line.IndexOf('=');
        if (idx <= 0) continue;
        var key = line[..idx].Trim();
        var val = line[(idx + 1)..].Trim();
        Environment.SetEnvironmentVariable(key, val);
    }
}

// Map ENV to config
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conn = builder.Configuration["CONNECTION_STRING"] 
           ?? builder.Configuration.GetConnectionString("Default");

if (string.IsNullOrWhiteSpace(conn))
    throw new Exception("Connection string not configured. Set CONNECTION_STRING or ConnectionStrings:Default");

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conn));

var tz = builder.Configuration["App:Timezone"] ?? "America/Sao_Paulo";
builder.Services.AddScoped(sp => new SlotService(sp.GetRequiredService<AppDbContext>(), tz));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("web", p =>
    {
        p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        var keyStr = builder.Configuration["JWT_KEY"] ?? "";
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT_ISSUER"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT_AUDIENCE"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("web");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Auto-migrate + seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Barbers.Any())
    {
        db.Barbers.AddRange(
            new BarberBooking.Api.Domain.Barber { Id = Guid.NewGuid(), Name = "Barbeiro 1", Active = true },
            new BarberBooking.Api.Domain.Barber { Id = Guid.NewGuid(), Name = "Barbeiro 2", Active = true }
        );
        db.SaveChanges();
    }
}

app.Run();
