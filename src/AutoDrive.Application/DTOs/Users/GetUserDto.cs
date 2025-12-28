namespace AutoDrive.Application.DTOs.Users;

public sealed record GetUserDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);
