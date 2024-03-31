using Common.Domain;
using MediatR;

namespace Todos.Application.Query.GetList;

public class GetTodoListQuery : IRequest<IReadOnlyCollection<Todo>>
{
    public int? Offset { get; set; }
    public string? LabelFreeText { get; set; }
    public int? OwnerId { get; set; }
    public int? Limit { get; set; } = 10;
}
