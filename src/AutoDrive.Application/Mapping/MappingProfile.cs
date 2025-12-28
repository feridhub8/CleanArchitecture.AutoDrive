using AutoDrive.Application.DTOs.Users;
using AutoDrive.Domain.Entities;
using AutoMapper;

namespace AutoDrive.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserDto, User>();
        CreateMap<User, UserClaimsDto>();
        CreateMap<User, GetUserDto>();
    }
}
