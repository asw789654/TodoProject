using Common.Application.Abstractions.Persistence;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Text.Json.Serialization;
using Todos.Application;
using Todos.Application.Query.GetById;

namespace Todos.Application.Query.GetCount;

public class GetTodoCountQueryHandler : IRequestHandler<GetTodoCountQuery, int>
{
    private readonly IRepository<Todo> _todos;
    private readonly MemoryCache _memoryCache;
    public GetTodoCountQueryHandler(
        IRepository<Todo> todos,
        TodosMemoryCache todosMemoryCache)
    {
        _todos = todos;
        _memoryCache = todosMemoryCache.Cache;
    }
    public async Task<int> Handle(GetTodoCountQuery request, CancellationToken cancellationToken = default)
    {
        var cashKey = JsonSerializer.Serialize(request, new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        });
        if (!_memoryCache.TryGetValue(cashKey, out int result))
        {
            return result;
        }

        result = await _todos.CountAsync(
            request.LabelFreeText == null ? null : b => b.Label.Contains(request.LabelFreeText, StringComparison.CurrentCultureIgnoreCase),
            request.OwnerId == null ? null : b => b.OwnerId == request.OwnerId,
            cancellationToken);

        var cacheEntryOptions = new MemoryCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromSeconds(45))
               .SetSlidingExpiration(TimeSpan.FromSeconds(30))
               .SetSize(3);

        _memoryCache.Set(cashKey, result, cacheEntryOptions);

        return result;
    }
}
