using AutoMapper;
using Common.Api.Services;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Serilog;
using Users.Application.Commands.AddUser;
using Users.Application.DTO;

namespace Users.Application.Commands.Delete;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRepository<ApplicationUser> _users;
    private readonly IMapper _mapper;

    public DeleteUserCommandHandler(
        ICurrentUserService currentUserService,
        IRepository<ApplicationUser> users,
        IMapper mapper)
    {
        _currentUserService = currentUserService;
        _users = users;
        _mapper = mapper;
    }
    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken = default)
    {
        bool isAdmin = _currentUserService.CurrentUserRoles().Contains("Admin");
        var userId = _users.SingleOrDefaultAsync(e => e.Id == request.Id).Result.Id;
        var currentUserId = _currentUserService.CurrentUserId();
        if (!isAdmin && currentUserId != userId)
        {
            throw new ForbiddenException();
        }

        var user = _mapper.Map<GetUserDto>(await _users.SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken));
        if (user == null)
        {
            Log.Error($"Incorrect user Id -{user}");
            throw new NotFoundException(new { request.Id });
        }

        var userEntity = new ApplicationUser()
        {
            Id = request.Id,
        };

        return await _users.DeleteAsync(userEntity, cancellationToken); ;

    }
}
