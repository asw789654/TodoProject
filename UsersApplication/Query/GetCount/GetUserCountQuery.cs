using MediatR;
using Users.Application.DTO;

namespace Users.Application.Query.GetCount;

public class GetUserCountQuery : IRequest<int>
{
    public string? NameFreeText { get; set; }
}
