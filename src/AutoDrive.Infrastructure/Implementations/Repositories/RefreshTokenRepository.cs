using AutoDrive.Application.Interfaces.Repositories;
using AutoDrive.Domain.Entities;
using AutoDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AutoDrive.Infrastructure.Implementations.Repositories;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(RefreshToken refreshToken)
    {
        _context.Add(refreshToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenWithUser(string token, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens.Include(r => r.User).FirstOrDefaultAsync(r => r.Token == token, cancellationToken);
    }
}
