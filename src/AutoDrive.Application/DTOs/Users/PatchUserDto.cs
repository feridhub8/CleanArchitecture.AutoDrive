namespace AutoDrive.Application.DTOs.Users;

public sealed record PatchUserDto(
    string? FirstName,
    string? LastName,
    string? Email,
    string? Password);
