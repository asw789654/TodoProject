using Common.Domain;
using Common.Repositories;
using Todos.BL.DTO;
using Todos.Domain;
using Todos.Repositories;

namespace Todos.BL;

public class TodoService : ITodoService
{
    private readonly IRepository<Todo> _todoRepository;
    private readonly IRepository<User> _usersRepository;

    public TodoService(IRepository<Todo> todoRepositity, IRepository<User> userRepository)
    {
        _usersRepository = userRepository;
        _todoRepository = todoRepositity;
        _todoRepository.Add(new Todo() { Id = 1, OwnerId = 3, Label = "Label 1", IsDone = true, CreatedDateTime = new DateTime(2024, 2, 26), UpdatedDate = new DateTime(2024, 2, 27) });
        _todoRepository.Add(new Todo() { Id = 2, OwnerId = 2, Label = "Label 2", IsDone = false, CreatedDateTime = new DateTime(2024, 1, 25), UpdatedDate = new DateTime(2024, 1, 27) });
        _todoRepository.Add(new Todo() { Id = 3, OwnerId = 1, Label = "Label 3", IsDone = false, CreatedDateTime = new DateTime(2024, 1, 24), UpdatedDate = new DateTime(2024, 1, 26) });
        _todoRepository.Add(new Todo() { Id = 4, OwnerId = 3, Label = "Label 4", IsDone = true, CreatedDateTime = new DateTime(2024, 2, 23), UpdatedDate = new DateTime(2024, 2, 27) });
        _todoRepository.Add(new Todo() { Id = 5, OwnerId = 2, Label = "Label 5", IsDone = false, CreatedDateTime = new DateTime(2024, 1, 22), UpdatedDate = new DateTime(2024, 1, 27) });
        _todoRepository.Add(new Todo() { Id = 6, OwnerId = 1, Label = "Label 6", IsDone = false, CreatedDateTime = new DateTime(2024, 1, 21), UpdatedDate = new DateTime(2024, 1, 26) });
        _todoRepository.Add(new Todo() { Id = 7, OwnerId = 3, Label = "Label 7", IsDone = true, CreatedDateTime = new DateTime(2024, 2, 20), UpdatedDate = new DateTime(2024, 2, 27) });
        _todoRepository.Add(new Todo() { Id = 8, OwnerId = 2, Label = "Label 8", IsDone = false, CreatedDateTime = new DateTime(2024, 1, 28), UpdatedDate = new DateTime(2024, 1, 27) });
        _todoRepository.Add(new Todo() { Id = 9, OwnerId = 1, Label = "Label 9", IsDone = false, CreatedDateTime = new DateTime(2024, 1, 27), UpdatedDate = new DateTime(2024, 1, 26) });
        _usersRepository.Add(new User() { Id = 1, Name = "name1" });
        _usersRepository.Add(new User() { Id = 2, Name = "name2" });
        _usersRepository.Add(new User() { Id = 3, Name = "name3" });
    }

    public IReadOnlyCollection<Todo> GetList(int? offset, string? labelFreeText, int? ownerId, int? limit = 10)
    {
        return _todoRepository.GetList(offset, limit,
            labelFreeText == null ? null : u => u.Label.Contains(labelFreeText, StringComparison.InvariantCulture),
            u => u.Id);
    }

    public Todo? GetById(int id)
    {
        return _todoRepository.SingleOrDefault(t => t.Id == id);
    }

    public Todo? AddToList(CreateTodoDto createTodoDto)
    {
        if (createTodoDto is null)
        {
            throw new Exception("Incorrect User");
        }
        var todoEntity = new Todo()
        {
            IsDone = createTodoDto.IsDone,
            OwnerId = createTodoDto.OwnerId,
            Label = createTodoDto.Label
        };
        return _todoRepository.Add(todoEntity); 
    }

    public Todo? Update(PutTodoDto putTodoDto)
    {
        var user = _usersRepository.SingleOrDefault(t => t.Id == putTodoDto.Id);
        if (user is null)
        {
            throw new Exception("Incorrect User");
        }
        var todoEntity = GetById(putTodoDto.Id);
        return _todoRepository.Update(todoEntity);
    }

    public int Count(string? labelFreeText) 
    {
        return _todoRepository.Count(labelFreeText == null
            ? null 
            : b => b.Label.Contains(labelFreeText,StringComparison.CurrentCultureIgnoreCase));
    }

    public Todo PatchIsDone(PatchIsDoneTodoDto patchIsDoneTodoDto)
    {
        var todoEntity = GetById(patchIsDoneTodoDto.Id);
        return _todoRepository.Update(todoEntity);
    }

    public bool Delete(RemoveTodoDto removeTodoDto)
    {
        var todoEntity = GetById(removeTodoDto.Id);
        if(todoEntity is null)
        {
            return false;
        }
        return _todoRepository.Delete(todoEntity);
    }
}
