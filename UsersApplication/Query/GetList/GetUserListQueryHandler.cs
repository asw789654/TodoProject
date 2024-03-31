using AutoMapper;
using Common.Application.Abstractions.Persistence;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Text.Json.Serialization;
using Users.Application.DTO;

namespace Users.Application.Query.GetList;

public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, IReadOnlyCollection<GetUserDto>>
{
    private readonly IRepository<ApplicationUser> _users;
    private readonly IMapper _mapper;
    private readonly MemoryCache _memoryCache;
    public GetUserListQueryHandler(
        IRepository<ApplicationUser> users,
        IMapper mapper,
        UsersMemoryCache usersMemoryCache)
    {
        _users = users;
        _mapper = mapper;
        _memoryCache = usersMemoryCache.Cache;
    }
    public async Task<IReadOnlyCollection<GetUserDto>> Handle(GetUserListQuery query, CancellationToken cancellationToken = default)
    {
        var cashKey = JsonSerializer.Serialize(query, new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        });
        if (_memoryCache.TryGetValue(cashKey, out IReadOnlyCollection<GetUserDto>? result))
        {
            return result;
        }
        result = _mapper.Map<IReadOnlyCollection<GetUserDto>>(await _users.GetListAsync(
            query.Offset,
            query.Limit,
            query.NameFreeText == null ? null : u => u.Name.Contains(query.NameFreeText, StringComparison.InvariantCulture),
            null,
            u => u.Id,
            false,
            cancellationToken));
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(45))
            .SetSlidingExpiration(TimeSpan.FromSeconds(30))
            .SetSize(3);

        _memoryCache.Set(cashKey, result, cacheEntryOptions);

        return result;
    }
}
