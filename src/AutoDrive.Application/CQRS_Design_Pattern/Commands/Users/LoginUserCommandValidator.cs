using AutoDrive.Application.DTOs.Users;
using FluentValidation;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.Users;

public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(u => u.LoginUserDto).SetValidator(new LoginUserDtoValidator());
    }
}
