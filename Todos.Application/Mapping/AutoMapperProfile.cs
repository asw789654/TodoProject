using AutoMapper;
using Common.Domain;
using Todos.Application.Commands.AddToList;
using Todos.Application.Commands.PatchIsDone;
using Todos.Application.Commands.Update;
using Todos.Application.Commands.Delete;

namespace Todos.Application.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AddTodoToListCommand, Todo>();
        CreateMap<PatchTodoIsDoneCommand, Todo>();
        CreateMap<UpdateTodoCommand, Todo>();
        CreateMap<DeleteTodoCommand, Todo>();
    }
}
