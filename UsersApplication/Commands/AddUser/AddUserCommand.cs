using MediatR;
using Users.Application.DTO;

namespace Users.Application.Commands.AddUser;

public class AddUserCommand : IRequest<GetUserDto>
{
    public string? Name { get; set; } = default;
    public string? Password { get; set; } = default;
}
