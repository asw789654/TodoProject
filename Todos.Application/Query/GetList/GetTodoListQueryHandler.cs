using Common.Api.Services;
using Common.Application.Abstractions.Persistence;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Todos.Application.Query.GetList;

public class GetTodoListQueryHandler : IRequestHandler<GetTodoListQuery, IReadOnlyCollection<Todo>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRepository<Todo> _todos;
    private readonly MemoryCache _memoryCache;
    public GetTodoListQueryHandler(
        ICurrentUserService currentUserService,
        IRepository<Todo> todos,
        TodosMemoryCache todosMemoryCache)
    {
        _currentUserService = currentUserService;
        _todos = todos;
        _memoryCache = todosMemoryCache.Cache;
    }
    public async Task<IReadOnlyCollection<Todo>> Handle(GetTodoListQuery query, CancellationToken cancellationToken = default)
    {
        bool isAdmin = _currentUserService.CurrentUserRoles().Contains("Admin");

        if (isAdmin)
        {
            var cashKey = JsonSerializer.Serialize(query, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
            if (_memoryCache.TryGetValue(cashKey, out IReadOnlyCollection<Todo>? result))
            {
                return result;
            }
            result = await _todos.GetListAsync(
            query.Offset,
            query.Limit,
            query.OwnerId == null ? null : t => t.OwnerId == query.OwnerId,
            query.LabelFreeText == null ? null : t => t.Label.Contains(query.LabelFreeText, StringComparison.InvariantCulture),
            t => t.Id,
            false,
            cancellationToken);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromSeconds(45))
               .SetSlidingExpiration(TimeSpan.FromSeconds(30))
               .SetSize(3);

            _memoryCache.Set(cashKey, result, cacheEntryOptions);
            return result;
        }
        else
        {
            var cashKey = JsonSerializer.Serialize(query, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
            if (_memoryCache.TryGetValue(cashKey, out IReadOnlyCollection<Todo>? result))
            {
                return result;
            }
            result = await _todos.GetListAsync(
            query.Offset,
            query.Limit,
            t => t.OwnerId == query.OwnerId,
            query.LabelFreeText == null ? null : t => t.Label.Contains(query.LabelFreeText, StringComparison.InvariantCulture) && t.OwnerId == _currentUserService.CurrentUserId(),
            t => t.Id,
            false,
            cancellationToken);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromSeconds(45))
               .SetSlidingExpiration(TimeSpan.FromSeconds(30))
               .SetSize(3);

            _memoryCache.Set(cashKey, result, cacheEntryOptions);

            return result;
        }
    }
}
