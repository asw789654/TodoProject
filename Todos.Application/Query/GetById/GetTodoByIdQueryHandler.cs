using Common.Api.Services;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Todos.Application.Query.GetById;

public class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, Todo>
{
    private readonly IRepository<Todo> _todos;
    private readonly ICurrentUserService _currentUserService;
    private readonly MemoryCache _memoryCache;

    public GetTodoByIdQueryHandler(
        IRepository<Todo> todos,
        ICurrentUserService currentUserService,
        TodosMemoryCache todosMemoryCache)
    {
        _todos = todos;
        _currentUserService = currentUserService;
        _memoryCache = todosMemoryCache.Cache;

    }

    public async Task<Todo?> Handle(GetTodoByIdQuery query, CancellationToken cancellationToken)
    {

        bool isAdmin = _currentUserService.CurrentUserRoles().Contains("Admin");

        var userOwnerId = _todos.SingleOrDefaultAsync(e => e.Id == query.Id).Result.OwnerId;
        var currentUserId = _currentUserService.CurrentUserId();
        if (!isAdmin && currentUserId != userOwnerId)
        {
            throw new ForbiddenException();
        }

        var cashKey = JsonSerializer.Serialize(query, new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        });
        if (!_memoryCache.TryGetValue(cashKey, out Todo? result))
        {
            return result;
        }

        result = await _todos.SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

        if (result == null)
        {
            Log.Error($"Incorrect id -{result}");
            throw new NotFoundException(new { query.Id });
        }

        var cacheEntryOptions = new MemoryCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromSeconds(45))
               .SetSlidingExpiration(TimeSpan.FromSeconds(30))
               .SetSize(3);

        _memoryCache.Set(cashKey, result, cacheEntryOptions);

        return result;
    }
}
