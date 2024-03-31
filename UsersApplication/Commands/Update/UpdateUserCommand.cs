using MediatR;
using Users.Application.DTO;

namespace Users.Application.Commands.Update;

public class UpdateUserCommand : IRequest<GetUserDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
}