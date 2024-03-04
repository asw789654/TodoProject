using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using Todos.BL.DTO;
using Todos.Domain;

namespace Todos.BL.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateTodoDto,Todo>();
        CreateMap<PatchIsDoneTodoDto, Todo>();
        CreateMap<PutTodoDto, Todo>();
        CreateMap<RemoveTodoDto, Todo>();
    }
}
