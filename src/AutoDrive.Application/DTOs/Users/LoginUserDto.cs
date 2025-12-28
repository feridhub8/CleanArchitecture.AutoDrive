namespace AutoDrive.Application.DTOs.Users;

public sealed record LoginUserDto(
    string Email,
    string Password);
