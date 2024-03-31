using Auth.Application.Utils;
using AutoMapper;
using Common.Api.Services;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Serilog;
using Users.Application.DTO;

namespace Users.Application.Commands.Update;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, GetUserDto>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRepository<ApplicationUser> _users;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(ICurrentUserService currentUserService,
        IRepository<ApplicationUser> users,
        IMapper mapper)
    {
        _currentUserService = currentUserService;
        _users = users;
        _mapper = mapper;
    }
    public async Task<GetUserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken = default)
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
            Name = request.Name,
            PasswordHash = PasswordHasher.HashPassword(request.Password)
        };
        _mapper.Map(request, userEntity);

        return _mapper.Map<GetUserDto>(await _users.UpdateAsync(userEntity, cancellationToken)); ;
    }
}
