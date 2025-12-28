namespace AutoDrive.Application.DTOs.Users;

public sealed record RegisterUserDto(
    string FirstName,
    string LastName,
    string Email,
    string Password);
