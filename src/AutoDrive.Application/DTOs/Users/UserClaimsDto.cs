using AutoDrive.Domain.Enums;

namespace AutoDrive.Application.DTOs.Users;

public record UserClaimsDto(
    Guid Id,
    string FirstName,
    string Lastname,
    string Email,
    UserRoles Role);
