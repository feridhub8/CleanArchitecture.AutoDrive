using AutoDrive.Application.Interfaces.Repositories;
using AutoDrive.Domain.Entities;
using AutoDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AutoDrive.Infrastructure.Implementations.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public void AddUser(User user)
    {
        _context.Add(user);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public Task<User?> GetUserByResetTokenAsync(Guid resetToken, CancellationToken cancellationToken)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.ResetToken == resetToken);
    }

    public void UpdateUser(User user)
    {
        _context.Update(user);
    }
}
