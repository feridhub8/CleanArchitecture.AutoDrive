using AutoDrive.Domain.Common;
using AutoDrive.Domain.Enums;

namespace AutoDrive.Domain.Entities;

public sealed class User : BaseEntity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public UserRoles Role { get; set; } = UserRoles.User;

    public Guid? ResetToken { get; set; }
    public DateTime? ResetTokenExpireTime { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
