using AutoDrive.Application.DTOs.Users;
using AutoDrive.Application.Interfaces.Helpers;
using AutoDrive.Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AutoDrive.Infrastructure.Implementations.Helpers;

public sealed class TokenGenerator : ITokenGenerator
{
    private readonly IConfiguration _configuration;

    public TokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public string GenerateJwtToken(UserClaimsDto userClaimsDto)
    {
        var jwtsettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtsettings["SecretKey"];
        var issuer = jwtsettings["Issuer"];
        var audience = jwtsettings["Audience"];
        if (!int.TryParse(jwtsettings["ExpirationInMinutes"], out var expirationInMinutes))
            throw new Exception("JWT ExpirationInMinutes is missing or invalid in configuration.");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userClaimsDto.Id.ToString()),
            new Claim(ClaimTypes.Name, userClaimsDto.FirstName),
            new Claim(ClaimTypes.Surname, userClaimsDto.Lastname),
            new Claim(ClaimTypes.Email, userClaimsDto.Email),
            new Claim(ClaimTypes.Role, userClaimsDto.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationInMinutes),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
