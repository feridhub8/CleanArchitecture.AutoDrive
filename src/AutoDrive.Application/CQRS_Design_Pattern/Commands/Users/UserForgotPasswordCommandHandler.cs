using AutoDrive.Application.Interfaces.UnitOfWork;
using FluentValidation;
using MediatR;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.Users;

public sealed class UserForgotPasswordCommandHandler : IRequestHandler<UserForgotPasswordCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UserForgotPasswordCommand> _validator;

    public UserForgotPasswordCommandHandler(IUnitOfWork unitOfWork, IValidator<UserForgotPasswordCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<string> Handle(UserForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var message = "Reset link sent to your email";
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var existingUser = await _unitOfWork.Users.GetUserByEmailAsync(request.UserForgotPasswordDto.Email, cancellationToken);
        if (existingUser == null)
            return message;

        existingUser.ResetToken = Guid.NewGuid();
        existingUser.ResetTokenExpireTime = DateTime.UtcNow.AddMinutes(15);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return message;
    }
}