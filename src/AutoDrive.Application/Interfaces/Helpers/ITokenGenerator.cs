using AutoDrive.Application.DTOs.Users;

namespace AutoDrive.Application.Interfaces.Helpers;

public interface ITokenGenerator
{
    string GenerateJwtToken(UserClaimsDto userClaimsDto);
    string GenerateRefreshToken();
}
