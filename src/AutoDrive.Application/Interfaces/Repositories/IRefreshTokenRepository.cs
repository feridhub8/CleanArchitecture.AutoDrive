using AutoDrive.Domain.Entities;

namespace AutoDrive.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    void Add(RefreshToken refreshToken);
    Task<RefreshToken?> GetRefreshTokenWithUser(string token, CancellationToken cancellationToken);
}
