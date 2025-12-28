using FluentValidation;

namespace AutoDrive.Application.DTOs.Users;

public sealed class UserForgotPasswordDtoValidator : AbstractValidator<UserForgotPasswordDto>
{
    public UserForgotPasswordDtoValidator()
    {
        RuleFor(u => u.Email).NotEmpty().WithMessage("Email cannot be empty")
                                 .EmailAddress().WithMessage("Invalid email address");
    }
}
