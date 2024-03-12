using AutoMapper;
using Common.Api.Exceptions;
using Common.Domain;
using Common.Repositories;
using Serilog;
using Todos.BL.DTO;

namespace Todos.BL;

public class TodoService : ITodoService
{
    private readonly IRepository<Todo> _todoRepository;
    private readonly IRepository<User> _usersRepository;
    private readonly IMapper _mapper;

    public TodoService(IMapper mapper, IRepository<Todo> todoRepositity, IRepository<User> userRepository)
    {
        _mapper = mapper;
        _usersRepository = userRepository;
        _todoRepository = todoRepositity;
    }

    public IReadOnlyCollection<Todo> GetList(int? offset, string? labelFreeText, int? ownerId, int? limit = 10)
    {
        return _todoRepository.GetList(offset, limit,
            ownerId == null ? null : t => t.OwnerId == ownerId,
            labelFreeText == null ? null : t => t.Label.Contains(labelFreeText, StringComparison.InvariantCulture),
            t => t.Id); ;
    }

    public Todo? GetById(int id)
    {
        var todo = _todoRepository.SingleOrDefault(t => t.Id == id);
        if (todo == null)
        {
            Log.Error($"Incorrect id -{todo}");
            throw new NotFoundException(new { Id = id });
        }
        return todo;
    }

    public Todo? AddToList(CreateTodoDto createTodoDto)
    {
        if (_usersRepository.SingleOrDefault(t => t.Id == createTodoDto.OwnerId) is null)
        {
            Log.Error($"Incorrect owner id -{createTodoDto}");
            throw new BadRequestException("Incorrect User id");
        }
        createTodoDto.Id = _todoRepository.Count() + 1;
        var todoEntity = _mapper.Map<Todo>(createTodoDto);
        return _todoRepository.Add(todoEntity);
    }

    public Todo? Update(PutTodoDto putTodoDto)
    {
        if (_usersRepository.SingleOrDefault(t => t.Id == putTodoDto.OwnerId) is null)
        {
            Log.Error($"Incorrect owner id -{putTodoDto.Id}");
            throw new BadRequestException("Incorrect User");
        }
        var todoEntity = GetById(putTodoDto.Id);
        _mapper.Map(putTodoDto, todoEntity);
        return _todoRepository.Update(todoEntity);
    }

    public int Count(string? labelFreeText, int? ownerId)
    {
        return _todoRepository.Count(
            labelFreeText == null ? null
            : b => b.Label.Contains(labelFreeText, StringComparison.CurrentCultureIgnoreCase),
            ownerId == null ? null
            : b => b.OwnerId == ownerId);
    }

    public Todo PatchIsDone(PatchIsDoneTodoDto patchIsDoneTodoDto)
    {
        var todoEntity = GetById(patchIsDoneTodoDto.Id);
        _mapper.Map(patchIsDoneTodoDto, todoEntity);
        return _todoRepository.Update(todoEntity);
    }

    public bool Delete(RemoveTodoDto removeTodoDto)
    {
        var todoEntity = GetById(removeTodoDto.Id);

        return _todoRepository.Delete(todoEntity);
    }

    Task<Todo?> ITodoService.GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _todoRepository.SingleOrDefauldAsync(p => p.Id == id, cancellationToken);
    }
}
