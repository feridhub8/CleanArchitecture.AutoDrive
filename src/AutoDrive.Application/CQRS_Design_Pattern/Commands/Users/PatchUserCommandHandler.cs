using AutoDrive.Application.Exceptions;
using AutoDrive.Application.Interfaces.Helpers;
using AutoDrive.Application.Interfaces.UnitOfWork;
using AutoDrive.Domain.Entities;
using FluentValidation;
using MediatR;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.Users;

public sealed class PatchUserCommandHandler : IRequestHandler<PatchUserCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<PatchUserCommand> _validator;

    public PatchUserCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IValidator<PatchUserCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _validator = validator;
    }

    public async Task<string> Handle(PatchUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.Users.GetUserByIdAsync(request.Id, cancellationToken);
        if (existingUser == null)
            throw new NotFoundException(nameof(User));

        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

        if (request.PatchUserDto.FirstName != null)
            existingUser.FirstName = request.PatchUserDto.FirstName;

        if (request.PatchUserDto.LastName != null)
        {
            existingUser.LastName = request.PatchUserDto.LastName;
        }

        if (request.PatchUserDto.Email != null)
        {
            if (existingUser.Email != request.PatchUserDto.Email)
            {
                var emailExists = await _unitOfWork.Users.GetUserByEmailAsync(request.PatchUserDto.Email, cancellationToken);
                if (emailExists != null)
                    throw new ConflictException("Email already used");
            }
            existingUser.Email = request.PatchUserDto.Email;
        }

        if (request.PatchUserDto.Password != null)
            existingUser.PasswordHash = _passwordHasher.HashPassword(request.PatchUserDto.Password);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "User patched successfully";
    }
}
