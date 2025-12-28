using AutoDrive.Application.DTOs.Users;
using MediatR;

namespace AutoDrive.Application.CQRS_Design_Pattern.Queries;

public sealed record GetUserByEmailQuery(string Email) : IRequest<GetUserDto>;
