using Common.Domain;
using MediatR;

namespace Todos.Application.Commands.AddToList;

public class AddTodoToListCommand : IRequest<Todo?>
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public string? Label { get; set; }
    public bool IsDone { get; set; }
}
