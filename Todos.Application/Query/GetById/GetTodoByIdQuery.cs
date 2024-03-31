using Common.Domain;
using MediatR;
using Todos.Application.DTO;

namespace Todos.Application.Query.GetById;

public class GetTodoByIdQuery : IRequest<Todo>
{
    public int Id { get; set; }
}
