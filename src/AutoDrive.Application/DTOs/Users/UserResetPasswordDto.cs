namespace AutoDrive.Application.DTOs.Users;

public sealed record UserResetPasswordDto(
    Guid ResetToken,
    string NewPassword);
