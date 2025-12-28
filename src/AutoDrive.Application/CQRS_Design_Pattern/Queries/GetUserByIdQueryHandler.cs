using AutoDrive.Application.DTOs.Users;
using AutoDrive.Application.Exceptions;
using AutoDrive.Application.Interfaces.UnitOfWork;
using AutoDrive.Domain.Entities;
using AutoMapper;
using MediatR;

namespace AutoDrive.Application.CQRS_Design_Pattern.Queries;

public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetUserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetUserByIdAsync(request.Id, cancellationToken);
        if (user == null)
            throw new NotFoundException(nameof(User));

        return _mapper.Map<GetUserDto>(user);
    }
}
