using AutoMapper;
using Common.Domain;
using Users.Application.Commands.AddUser;
using Users.Application.Commands.Delete;
using Users.Application.Commands.Update;
using Users.Application.Commands.UpdatePassword;
using Users.Application.DTO;

namespace Users.Application.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AddUserCommand, ApplicationUser>();
        CreateMap<UpdateUserCommand, ApplicationUser>();
        CreateMap<DeleteUserCommand, ApplicationUser>();
        CreateMap<UpdateUserPasswordCommand, ApplicationUser>();
        CreateMap<ApplicationUser, GetUserDto>();
    }
}
