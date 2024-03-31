using Common.Application.Abstractions.Persistence;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Users.Application.Query.GetCount;

public class GetUserCountQueryHandler : IRequestHandler<GetUserCountQuery, int>
{
    private readonly IRepository<ApplicationUser> _users;
    private readonly MemoryCache _memoryCache;
    public GetUserCountQueryHandler(
        IRepository<ApplicationUser> users,
        UsersMemoryCache usersMemoryCache)
    {
        _users = users;
        _memoryCache = usersMemoryCache.Cache;
    }

    public async Task<int> Handle(GetUserCountQuery query, CancellationToken cancellationToken)
    {
        var cashKey = JsonSerializer.Serialize($"Count:{query}", new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        });

        if (!_memoryCache.TryGetValue(cashKey, out int? result))
        {
            return result!.Value;
        }

        if (string.IsNullOrWhiteSpace(query.NameFreeText))
        {
            result = await _users.CountAsync(cancellationToken: cancellationToken);
        }
        else
        {
            result = await _users.CountAsync(u => u.Name.Contains(query.NameFreeText), cancellationToken: cancellationToken);
        }
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(45))
            .SetSlidingExpiration(TimeSpan.FromSeconds(30))
            .SetSize(1);

        _memoryCache.Set(cashKey, result, cacheEntryOptions);

        return result.Value;
    }
}
