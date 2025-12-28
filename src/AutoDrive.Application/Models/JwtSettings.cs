namespace AutoDrive.Application.Models;

public sealed record JwtSettings(
    string SecretKey,
    string Issuer,
    string Audience,
    int ExpirationInMinutes
);
