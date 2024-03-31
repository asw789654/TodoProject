using AutoMapper;
using Common.Api.Services;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Serilog;
using Todos.Application.Commands.Delete;

namespace Todos.Application.Commands.PatchIsDone;

public class PatchTodoIsDoneCommandHandler : IRequestHandler<PatchTodoIsDoneCommand, Todo>
{
    private readonly IRepository<Todo> _todos;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public PatchTodoIsDoneCommandHandler(
        IRepository<Todo> todos,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _todos = todos;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    public async Task<Todo> Handle(PatchTodoIsDoneCommand request, CancellationToken cancellationToken = default)
    {
        bool isAdmin = _currentUserService.CurrentUserRoles().Contains("Admin");

        var userOwnerId = _todos.SingleOrDefaultAsync(e => e.Id == request.Id).Result.OwnerId;
        var currentUserId = _currentUserService.CurrentUserId();
        if (!isAdmin && currentUserId != userOwnerId)
        {
            throw new ForbiddenException();
        }
        var todoEntity = await _todos.SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (todoEntity == null)
        {
            Log.Error($"Incorrect id -{todoEntity}");
            throw new NotFoundException(new { Id = request.Id });
        }

        _mapper.Map(request, todoEntity);

        return await _todos.UpdateAsync(todoEntity, cancellationToken);
    }
}
