using AutoDrive.Domain.Entities;

namespace AutoDrive.Application.Interfaces.Repositories;

public interface IUserRepository
{
    void AddUser(User user);
    void UpdateUser(User user);
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetUserByResetTokenAsync(Guid resetToken, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken);
}
