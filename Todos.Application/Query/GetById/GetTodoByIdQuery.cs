using Common.Domain;
using MediatR;

namespace Todos.Application.Query.GetById;

public class GetTodoByIdQuery : IRequest<Todo>
{
    public int Id { get; set; }
}
