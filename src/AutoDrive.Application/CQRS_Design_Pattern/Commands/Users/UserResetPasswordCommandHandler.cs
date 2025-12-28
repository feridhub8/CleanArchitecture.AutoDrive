using AutoDrive.Application.Exceptions;
using AutoDrive.Application.Interfaces.Helpers;
using AutoDrive.Application.Interfaces.UnitOfWork;
using FluentValidation;
using MediatR;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.Users;

public class UserResetPasswordCommandHandler : IRequestHandler<UserResetPasswordCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<UserResetPasswordCommand> _validator;

    public UserResetPasswordCommandHandler(IUnitOfWork unitOfWork, UserResetPasswordCommandValidator validator, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> Handle(UserResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var existingUser = await _unitOfWork.Users.GetUserByResetTokenAsync(request.UserResetPasswordDto.ResetToken, cancellationToken);

        if (existingUser == null || existingUser.ResetTokenExpireTime < DateTime.UtcNow)
            throw new BadRequestException("Reset token is invalid or expired");

        existingUser.PasswordHash = _passwordHasher.HashPassword(request.UserResetPasswordDto.NewPassword);

        existingUser.ResetToken = null;
        existingUser.ResetTokenExpireTime = null;
        existingUser.UpdatedDate = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Password successfully reset";
    }
}
