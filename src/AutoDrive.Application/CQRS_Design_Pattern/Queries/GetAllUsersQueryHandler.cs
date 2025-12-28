using AutoDrive.Application.DTOs.Users;
using AutoDrive.Application.Interfaces.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AutoDrive.Application.CQRS_Design_Pattern.Queries;

public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<GetUserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetUserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.Users.GetAllUsersAsync(cancellationToken);
        return _mapper.Map<IEnumerable<GetUserDto>>(users);
    }
}
