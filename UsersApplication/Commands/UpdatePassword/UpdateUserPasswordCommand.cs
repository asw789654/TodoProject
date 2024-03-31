using MediatR;
using Users.Application.DTO;

namespace Users.Application.Commands.UpdatePassword;

public class UpdateUserPasswordCommand : IRequest<GetUserDto>
{
    public int Id { get; set; }
    public string Password { get; set; }
}
