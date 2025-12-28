using AutoDrive.Application.Exceptions;
using AutoDrive.Application.Interfaces.Helpers;
using AutoDrive.Application.Interfaces.UnitOfWork;
using AutoDrive.Domain.Entities;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.Users;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<RegisterUserCommand> _validator;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher, IValidator<RegisterUserCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _validator = validator;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var exist = await _unitOfWork.Users.GetUserByEmailAsync(request.RegisterUserDto.Email, cancellationToken);
        if (exist != null)
            throw new ConflictException("User with this email already exists");

        var user = _mapper.Map<User>(request.RegisterUserDto);

        user.PasswordHash = _passwordHasher.HashPassword(request.RegisterUserDto.Password);

        _unitOfWork.Users.AddUser(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
