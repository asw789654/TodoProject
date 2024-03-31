using AutoMapper;
using Common.Domain;
using MediatR;
using Auth.Application.Utils;
using Users.Application.DTO;
using Common.Application.Exceptions;
using Common.Application.Abstractions.Persistence;

namespace Users.Application.Commands.AddUser;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, GetUserDto>
{
    private readonly IRepository<ApplicationUser> _users;
    private readonly IRepository<ApplicationUserRole> _roles;
    private readonly IMapper _mapper;

    public AddUserCommandHandler(
        IRepository<ApplicationUser> users,
        IRepository<ApplicationUserRole> roles,
        IMapper mapper)
    {
        _users = users;
        _roles = roles;
        _mapper = mapper;
    }
    public async Task<GetUserDto?> Handle(AddUserCommand request, CancellationToken cancellationToken = default)
    {
        if (await _users.SingleOrDefaultAsync(u => u.Name == request.Name.Trim()) is not null)
        {
            throw new ForbiddenException();
        }
        var userRole = (await _roles.SingleOrDefaultAsync(r => r.Name == "Admin", cancellationToken))!;
        //var userEntity = _mapper.Map<User>(addUserDto);
        var userEntity = new ApplicationUser()
        {
            Name = request.Name,
            PasswordHash = PasswordHasher.HashPassword(request.Password),
            Roles = new[] { new ApplicationUserApplicationRole() { ApplicationUserRoleId = userRole.Id } }
        };

        userEntity.Id = _users.CountAsync(cancellationToken: cancellationToken).Result + 1;

        return _mapper.Map<GetUserDto>(await _users.AddAsync(userEntity, cancellationToken));
    }
}
