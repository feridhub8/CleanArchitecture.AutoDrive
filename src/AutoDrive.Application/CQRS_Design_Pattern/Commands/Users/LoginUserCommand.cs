using AutoDrive.Application.DTOs.Response;
using AutoDrive.Application.DTOs.Users;
using MediatR;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.Users;

public sealed record LoginUserCommand(LoginUserDto LoginUserDto) : IRequest<LoginResponse>;
