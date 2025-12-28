using AutoDrive.Application.DTOs.Users;
using FluentValidation;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.Users;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.RegisterUserDto).SetValidator(new RegisterUserDtoValidator());
    }
}
