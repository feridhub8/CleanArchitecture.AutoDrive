namespace AutoDrive.Domain.Entities;

public sealed class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = default!;
    public DateTime Expires { get; set; }
    public bool IsRevoked { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
}
