using AutoDrive.Application.DTOs.Response;
using AutoDrive.Application.DTOs.Users;
using AutoDrive.Application.Interfaces.Helpers;
using AutoDrive.Application.Interfaces.Repositories;
using AutoDrive.Application.Interfaces.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.RefreshTokens;

public class RefreshTokenLoginCommandHandler : IRequestHandler<RefreshTokenLoginCommand, LoginResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RefreshTokenLoginCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        ITokenGenerator tokenGenerator,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<LoginResponse> Handle(RefreshTokenLoginCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetRefreshTokenWithUser(request.RefreshTokenDto.Token, cancellationToken);
        if (refreshToken == null || refreshToken.IsRevoked || refreshToken.Expires < DateTime.UtcNow)
            throw new Exception("Invalid or expired refresh token.");

        var user = await _userRepository.GetUserByIdAsync(refreshToken.UserId, cancellationToken);
        if (user == null)
            throw new Exception("User not found.");

        var userClaimsDto = _mapper.Map<UserClaimsDto>(user);
        var newAccessToken = _tokenGenerator.GenerateJwtToken(userClaimsDto);
        var newRefreshToken = _tokenGenerator.GenerateRefreshToken();
        refreshToken.Token = newRefreshToken;
        refreshToken.Expires = DateTime.UtcNow.AddDays(7);
        refreshToken.IsRevoked = false;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new LoginResponse(newAccessToken, newRefreshToken);

    }
}
