using CustomerManagement.Application.Interfaces.Services;
using CustomerManagement.Common.Constants;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomerManagement.Infrastructure.Services;

public class AuthService : IAuthService
{
    private const string AdminEmail = "tieugiaacademy@yopmail.com";
    private const string AdminPassword = "123456@abc";

    public string? Login(string email, string password)
    {
        if (email != AdminEmail || password != AdminPassword)
            return null;

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKeyJwt.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "CustomerAPI",
            audience: "CustomerClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
