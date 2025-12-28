using MediatR;

namespace AutoDrive.Application.CQRS_Design_Pattern.Commands.Users;

public sealed record DeleteUserCommand(Guid Id) : IRequest<string>;
