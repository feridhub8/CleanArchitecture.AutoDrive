using AutoDrive.Application.DTOs.Users;
using FluentValidation;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.Users;

public sealed class PatchUserCommandValidator : AbstractValidator<PatchUserCommand>
{
    public PatchUserCommandValidator()
    {
        RuleFor(u => u.PatchUserDto).SetValidator(new PatchUserDtoValidator());
    }
}
