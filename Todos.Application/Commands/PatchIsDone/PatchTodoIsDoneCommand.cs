using Common.Domain;
using MediatR;

namespace Todos.Application.Commands.PatchIsDone;

public class PatchTodoIsDoneCommand : IRequest<Todo>
{
    public int Id { get; set; }
    public bool IsDone { get; set; }
}
