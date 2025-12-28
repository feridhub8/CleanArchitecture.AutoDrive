using AutoDrive.Application.Interfaces.Repositories;
using AutoDrive.Application.Interfaces.UnitOfWork;
using AutoDrive.Infrastructure.Implementations.Repositories;
using AutoDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace AutoDrive.Infrastructure.Implementations.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _context = context;
        Users = userRepository;
        RefreshTokens = refreshTokenRepository;
    }

    private IDbContextTransaction? _transaction;
    public IUserRepository Users { get; }
    public IRefreshTokenRepository RefreshTokens { get; }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
