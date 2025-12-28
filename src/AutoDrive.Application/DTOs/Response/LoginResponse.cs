namespace AutoDrive.Application.DTOs.Response;

public sealed record LoginResponse(
    string JwtToken,
    string RefreshToken);
