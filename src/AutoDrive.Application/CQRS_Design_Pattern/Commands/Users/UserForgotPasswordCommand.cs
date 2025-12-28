using AutoDrive.Application.DTOs.Users;
using MediatR;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.Users;

public sealed record UserForgotPasswordCommand(UserForgotPasswordDto UserForgotPasswordDto) : IRequest<string>
{
}
