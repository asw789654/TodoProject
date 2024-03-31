using MediatR;
using Users.Application.DTO;

namespace Users.Application.Commands.Delete;

public class DeleteUserCommand : IRequest<bool>
{
    public int Id { get; set; }
}
