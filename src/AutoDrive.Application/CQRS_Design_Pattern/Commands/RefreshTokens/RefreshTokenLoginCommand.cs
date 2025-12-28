using AutoDrive.Application.DTOs.RefreshToken;
using AutoDrive.Application.DTOs.Response;
using MediatR;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.RefreshTokens;

public sealed record RefreshTokenLoginCommand(RefreshTokenDto RefreshTokenDto) : IRequest<LoginResponse>;
