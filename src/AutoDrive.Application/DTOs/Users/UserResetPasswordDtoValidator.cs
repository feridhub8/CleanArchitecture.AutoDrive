using FluentValidation;

namespace AutoDrive.Application.DTOs.Users;

public sealed class UserResetPasswordDtoValidator : AbstractValidator<UserResetPasswordDto>
{
    public UserResetPasswordDtoValidator()
    {
        RuleFor(u => u.NewPassword).NotEmpty().WithMessage("Password cannot be empty")
                            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                            .MaximumLength(30).WithMessage("Password must be at most 30 characters long")
                            .Matches(@"^(?=.*[A-Z])(?=.*\d).+$")
                            .WithMessage("Password must contain at least one uppercase letter and one digit"); ;
    }
}
