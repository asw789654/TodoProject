using Common.Api.Services;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Serilog;

namespace Todos.Application.Commands.Delete;

public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, bool>
{
    private readonly IRepository<Todo> _todos;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTodoCommandHandler(
        IRepository<Todo> todos,
        ICurrentUserService currentUserService)
    {
        _todos = todos;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(
        DeleteTodoCommand request,
        CancellationToken cancellationToken = default)
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

        return await _todos.DeleteAsync(todoEntity, cancellationToken);
    }
}
