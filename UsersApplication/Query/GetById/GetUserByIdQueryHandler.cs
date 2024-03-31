using AutoMapper;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using Users.Application.DTO;

namespace Users.Application.Query.GetById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserDto>
{
    private readonly IRepository<ApplicationUser> _users;
    private readonly IMapper _mapper;
    private readonly MemoryCache _memoryCache;

    public GetUserByIdQueryHandler(
        IRepository<ApplicationUser> users,
        IMapper mapper,
        UsersMemoryCache usersMemoryCache)
    {
        _users = users;
        _mapper = mapper;
        _memoryCache = usersMemoryCache.Cache;
    }
    public async Task<GetUserDto?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        var cashKey = JsonSerializer.Serialize(query, new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        });

        if (!_memoryCache.TryGetValue(cashKey, out GetUserDto? result))
        {
            return result;
        }
        result = _mapper.Map<GetUserDto>(await _users.SingleOrDefaultAsync(t => t.Id == query.Id, cancellationToken));
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
