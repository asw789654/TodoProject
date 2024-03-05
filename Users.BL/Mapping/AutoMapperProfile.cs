using AutoMapper;
using Common.Domain;
using Users.BL.DTO;

namespace Todos.BL.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AddUserDto,User>();
        CreateMap<UpdateUserDto, User>();
    }
}
