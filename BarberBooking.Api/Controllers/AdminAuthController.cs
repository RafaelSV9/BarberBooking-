using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BarberBooking.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BarberBooking.Api.Controllers;

[ApiController]
[Route("api/admin/auth")]
public class AdminAuthController : ControllerBase
{
    private readonly IConfiguration _cfg;
    public AdminAuthController(IConfiguration cfg) => _cfg = cfg;

    [HttpPost("login")]
    public ActionResult<AdminLoginResponse> Login([FromBody] AdminLoginRequest req)
    {
        var adminUser = _cfg["ADMIN_USER"];
        var adminPass = _cfg["ADMIN_PASSWORD"];

        if (string.IsNullOrWhiteSpace(adminUser) || string.IsNullOrWhiteSpace(adminPass))
            return Problem("ADMIN_USER / ADMIN_PASSWORD not configured");

        if (req.User != adminUser || req.Password != adminPass)
            return Unauthorized("Credenciais inválidas");

        var keyStr = _cfg["JWT_KEY"];
        if (string.IsNullOrWhiteSpace(keyStr) || keyStr.Length < 32)
            return Problem("JWT_KEY must be configured (32+ chars)");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _cfg["JWT_ISSUER"],
            audience: _cfg["JWT_AUDIENCE"],
            claims: new[] { new Claim("role", "admin") },
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new AdminLoginResponse(jwt));
    }
}
