using Common.Domain;
using MediatR;

namespace Todos.Application.Query.GetCount;

public class GetTodoCountQuery : IRequest<int>
{
    public string? LabelFreeText { get; set; }
    public int? OwnerId { get; set; }
}
