using AutoDrive.Application.DTOs.Response;
using AutoDrive.Application.DTOs.Users;
using AutoDrive.Application.Interfaces.Helpers;
using AutoDrive.Application.Interfaces.UnitOfWork;
using AutoDrive.Domain.Entities;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.Users
{
    public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IValidator<LoginUserCommand> _validator;
        private readonly IMapper _mapper;

        public LoginUserCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, IValidator<LoginUserCommand> validator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<LoginResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var user = await _unitOfWork.Users.GetUserByEmailAsync(request.LoginUserDto.Email, cancellationToken);
            if (user == null || !_passwordHasher.VerifyPassword(request.LoginUserDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Email or password is not correct");

            var userClaimsDto = _mapper.Map<UserClaimsDto>(user);
            var accessToken = _tokenGenerator.GenerateJwtToken(userClaimsDto);

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = _tokenGenerator.GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(7)
            };

            _unitOfWork.RefreshTokens.Add(refreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new LoginResponse(accessToken, refreshToken.Token);
        }
    }
}
