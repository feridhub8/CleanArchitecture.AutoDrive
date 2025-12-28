using AutoDrive.Application.DTOs.Users;
using FluentValidation;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.Users;

public sealed class UserResetPasswordCommandValidator : AbstractValidator<UserResetPasswordCommand>
{
    public UserResetPasswordCommandValidator()
    {
        RuleFor(u => u.UserResetPasswordDto).SetValidator(new UserResetPasswordDtoValidator());
    }
}
