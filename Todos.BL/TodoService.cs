using AutoMapper;
using Common.Api.Exceptions;
using Common.Api.Services;
using Common.Domain;
using Common.Repositories;
using Serilog;
using Todos.BL.DTO;

namespace Todos.BL;

public class TodoService : ITodoService
{
    private readonly IRepository<Todo> _todoRepository;
    private readonly IRepository<ApplicationUser> _usersRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public TodoService(IMapper mapper, IRepository<Todo> todoRepositity, IRepository<ApplicationUser> userRepository, ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _usersRepository = userRepository;
        _todoRepository = todoRepositity;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyCollection<Todo>> GetListAsync(
        int? offset,
        string? labelFreeText,
        int? ownerId,
        int? limit = 10,
        CancellationToken cancellationToken = default)
    {
        bool isAdmin = _currentUserService.CurrentUserRoles().Contains("Admin");
        if (isAdmin)
        {
            return await _todoRepository.GetListAsync(
            offset,
            limit,
            ownerId == null ? null : t => t.OwnerId == ownerId,
            labelFreeText == null ? null : t => t.Label.Contains(labelFreeText, StringComparison.InvariantCulture),
            t => t.Id,
            false,
            cancellationToken);
        }
        return await _todoRepository.GetListAsync(
            offset,
            limit,
            ownerId == null ? null : t => t.OwnerId == ownerId,
            labelFreeText == null ? null : t => t.Label.Contains(labelFreeText, StringComparison.InvariantCulture) && t.OwnerId == _currentUserService.CurrentUserId(),
            t => t.Id,
            false,
            cancellationToken);
    }

    public async Task<Todo?> AddToListAsync(CreateTodoDto createTodoDto, CancellationToken cancellationToken = default)
    {
        createTodoDto.Id = _todoRepository.CountAsync(cancellationToken: cancellationToken).Result + 1;
        var todoEntity = _mapper.Map<Todo>(createTodoDto);
        todoEntity.OwnerId = _currentUserService.CurrentUserId();
        return await _todoRepository.AddAsync(todoEntity, cancellationToken);
    }

    public async Task<Todo?> UpdateAsync(PutTodoDto putTodoDto, CancellationToken cancellationToken = default)
    {
        var todoEntity = GetByIdAsync(putTodoDto.Id, cancellationToken).Result;

        return await _todoRepository.UpdateAsync(todoEntity, cancellationToken);
    }

    public async Task<int> CountAsync(string? labelFreeText, int? ownerId, CancellationToken cancellationToken = default)
    {
        return await _todoRepository.CountAsync(
            labelFreeText == null ? null : b => b.Label.Contains(labelFreeText, StringComparison.CurrentCultureIgnoreCase),
            ownerId == null ? null : b => b.OwnerId == ownerId,
            cancellationToken);
    }

    public async Task<Todo> PatchIsDoneAsync(PatchIsDoneTodoDto patchIsDoneTodoDto, CancellationToken cancellationToken = default)
    {
        var todoEntity = GetByIdAsync(patchIsDoneTodoDto.Id, cancellationToken).Result;
        _mapper.Map(patchIsDoneTodoDto, todoEntity);

        return await _todoRepository.UpdateAsync(todoEntity, cancellationToken);
    }

    public async Task<bool> DeleteAsync(RemoveTodoDto removeTodoDto, CancellationToken cancellationToken = default)
    {
        var todoEntity = GetByIdAsync(removeTodoDto.Id, cancellationToken).Result;

        return await _todoRepository.DeleteAsync(todoEntity, cancellationToken);
    }

    public async Task<Todo?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {

        bool isAdmin = _currentUserService.CurrentUserRoles().Contains("Admin");
        var userOwnerId = _todoRepository.SingleOrDefaultAsync(e => e.Id == id).Result.OwnerId;
        var currentUserId = _currentUserService.CurrentUserId();
        if (!isAdmin && currentUserId != userOwnerId)
        {
            throw new BadRequestException("Access denied");
        }
        var todo = await _todoRepository.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (todo == null)
        {
            Log.Error($"Incorrect id -{todo}");
            throw new NotFoundException(new { Id = id });
        }

        return todo;
    }
}
