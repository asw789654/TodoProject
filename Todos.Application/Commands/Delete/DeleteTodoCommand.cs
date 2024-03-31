using Common.Domain;
using MediatR;
using Todos.Application.DTO;

namespace Todos.Application.Commands.Delete;

public class DeleteTodoCommand : IRequest<bool>
{
    public int Id { get; set; }
}
