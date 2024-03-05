using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Todos.BL.DTO;
using Todos.Domain;

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
        if (_usersRepository.Count() == 0)
        {
            _usersRepository.Add(new User() { Id = 1, Name = "name1" });
            _usersRepository.Add(new User() { Id = 2, Name = "name2" });
            _usersRepository.Add(new User() { Id = 3, Name = "name3" });
        }
        if (_todoRepository.Count() == 0)
        {
            _todoRepository.Add(new Todo() { Id = 1, OwnerId = 3, Label = "Label 1", IsDone = true, CreatedDateTime = new DateTime(2024, 2, 26), UpdatedDate = new DateTime(2024, 2, 27) });
            _todoRepository.Add(new Todo() { Id = 2, OwnerId = 2, Label = "Label 2", IsDone = false, CreatedDateTime = new DateTime(2024, 1, 25), UpdatedDate = new DateTime(2024, 1, 27) });
            _todoRepository.Add(new Todo() { Id = 3, OwnerId = 1, Label = "Label 3", IsDone = false, CreatedDateTime = new DateTime(2024, 1, 24), UpdatedDate = new DateTime(2024, 1, 26) });
            _todoRepository.Add(new Todo() { Id = 4, OwnerId = 3, Label = "Label 4", IsDone = true, CreatedDateTime = new DateTime(2024, 2, 23), UpdatedDate = new DateTime(2024, 2, 27) });
            _todoRepository.Add(new Todo() { Id = 5, OwnerId = 2, Label = "Label 5", IsDone = false, CreatedDateTime = new DateTime(2024, 1, 22), UpdatedDate = new DateTime(2024, 1, 27) });
            _todoRepository.Add(new Todo() { Id = 6, OwnerId = 1, Label = "Label 6", IsDone = false, CreatedDateTime = new DateTime(2024, 1, 21), UpdatedDate = new DateTime(2024, 1, 26) });
            _todoRepository.Add(new Todo() { Id = 7, OwnerId = 3, Label = "Label 7", IsDone = true, CreatedDateTime = new DateTime(2024, 2, 20), UpdatedDate = new DateTime(2024, 2, 27) });
            _todoRepository.Add(new Todo() { Id = 8, OwnerId = 2, Label = "Label 8", IsDone = false, CreatedDateTime = new DateTime(2024, 1, 28), UpdatedDate = new DateTime(2024, 1, 27) });
            _todoRepository.Add(new Todo() { Id = 9, OwnerId = 1, Label = "Label 9", IsDone = false, CreatedDateTime = new DateTime(2024, 1, 27), UpdatedDate = new DateTime(2024, 1, 26) });

        }
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
        return _todoRepository.SingleOrDefault(t => t.Id == id);
    }

    public Todo? AddToList(CreateTodoDto createTodoDto)
    {
        if (_usersRepository.SingleOrDefault(t => t.Id == createTodoDto.OwnerId) is null)
        {
            throw new Exception("Incorrect User");
        }
        createTodoDto.Id = _todoRepository.Count() + 1;
        var todoEntity = _mapper.Map<Todo>(createTodoDto);
        return _todoRepository.Add(todoEntity);
    }

    public Todo? Update(PutTodoDto putTodoDto)
    {
        var user = _usersRepository.SingleOrDefault(t => t.Id == putTodoDto.OwnerId);
        if (user is null)
        {
            throw new Exception("Incorrect User");
        }

        var todoEntity = _todoRepository.SingleOrDefault(t => t.Id == putTodoDto.Id);
        _mapper.Map(putTodoDto, todoEntity);
        return _todoRepository.Update(todoEntity);
    }

    public int Count(string? labelFreeText,int? ownerId)
    {
        return _todoRepository.Count(
            labelFreeText == null ? null
            : b => b.Label.Contains(labelFreeText, StringComparison.CurrentCultureIgnoreCase),
            ownerId == null ? null
            : b => b.OwnerId == ownerId);
    }

    public Todo PatchIsDone(PatchIsDoneTodoDto patchIsDoneTodoDto)
    {
        var todoEntity = _todoRepository.SingleOrDefault(t => t.Id == patchIsDoneTodoDto.Id);
        _mapper.Map(patchIsDoneTodoDto, todoEntity);
        return _todoRepository.Update(todoEntity);
    }

    public bool Delete(RemoveTodoDto removeTodoDto)
    {
        var todoEntity = GetById(removeTodoDto.Id);
        if (todoEntity is null)
        {
            return false;
        }
        return _todoRepository.Delete(todoEntity);
    }
}
