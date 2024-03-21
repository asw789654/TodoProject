using AutoMapper;
using Common.Domain;
using Users.BL.DTO;

namespace Users.BL.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AddUserDto,ApplicationUser>();
        CreateMap<UpdateUserDto, ApplicationUser>();
        CreateMap<UpdateUserPasswordDto, ApplicationUser>();
        CreateMap<ApplicationUser,GetUserDto>();
    }
}
