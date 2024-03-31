using MediatR;
using Users.Application.DTO;

namespace Users.Application.Query.GetList;

public class GetUserListQuery : IRequest<IReadOnlyCollection<GetUserDto>>
{
    public string? NameFreeText { get; set; }
    public int? Offset { get; set; }

    public int? Limit { get; set; }
}
