using MediatR;
using Users.Application.DTO;

namespace Users.Application.Query.GetById;

public class GetUserByIdQuery : IRequest<GetUserDto>
{
    public int Id { get; set; }
}
