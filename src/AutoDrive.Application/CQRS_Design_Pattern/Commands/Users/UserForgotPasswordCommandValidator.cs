using AutoDrive.Application.DTOs.Users;
using FluentValidation;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.Users;

public sealed class UserForgotPasswordCommandValidator : AbstractValidator<UserForgotPasswordCommand>
{
    public UserForgotPasswordCommandValidator()
    {
        RuleFor(u => u.UserForgotPasswordDto).SetValidator(new UserForgotPasswordDtoValidator());
    }
}
