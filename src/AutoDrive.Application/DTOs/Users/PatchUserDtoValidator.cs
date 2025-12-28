using FluentValidation;

namespace AutoDrive.Application.DTOs.Users;

public sealed class PatchUserDtoValidator : AbstractValidator<PatchUserDto>
{
    public PatchUserDtoValidator()
    {
        RuleFor(u => u).Must(u => u.FirstName is not null
                               || u.LastName is not null
                               || u.Email is not null
                               || u.Password is not null)
            .WithMessage("At least one field must be provided");

        RuleFor(u => u.FirstName).NotEmpty().WithMessage("First name cannot be empty")
                                 .MinimumLength(2).WithMessage("First name must be at least 2 characters long")
                                 .MaximumLength(20).WithMessage("First name must be at most 20 characters long")
                                 .When(u => u.FirstName is not null);

        RuleFor(u => u.LastName).NotEmpty().WithMessage("Last name cannot be empty")
                                 .MinimumLength(2).WithMessage("Last name must be at least 2 characters long")
                                 .MaximumLength(20).WithMessage("Last name must be at most 20 characters long")
                                 .When(u => u.LastName is not null);

        RuleFor(u => u.Email).NotEmpty().WithMessage("Email cannot be empty")
                                 .EmailAddress().WithMessage("Invalid email address")
                                 .When(u => u.Email is not null);

        RuleFor(u => u.Password).NotEmpty().WithMessage("Password cannot be empty")
                                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                                .MaximumLength(30).WithMessage("Password must be at most 30 characters long")
                                .Matches(@"^(?=.*[A-Z])(?=.*\d).+$")
                                .WithMessage("Password must contain at least one uppercase letter and one digit")
                                .When(u => u.Password is not null);
    }
}
