using AutoDrive.Application.Exceptions;
using AutoDrive.Application.Interfaces.UnitOfWork;
using AutoDrive.Domain.Entities;
using MediatR;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.Users;

public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.Users.GetUserByIdAsync(request.Id, cancellationToken);
        if (existingUser == null)
            throw new NotFoundException(nameof(User));

        existingUser.IsDeleted = true;
        existingUser.DeletedDate = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "User deleted successfully";
    }
}