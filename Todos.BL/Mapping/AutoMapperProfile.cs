using AutoMapper;
using Common.Domain;
using Todos.BL.DTO;

namespace Todos.BL.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateTodoDto,Todo>();
        CreateMap<PatchIsDoneTodoDto, Todo>();
        CreateMap<PutTodoDto, Todo>();
    }
}
