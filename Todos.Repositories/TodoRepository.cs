using Todos.Domain;

namespace Todos.Repositories;

public class TodoRepository : ITodoRepository
{
    public static List<Todo> Todos = new List<Todo>(){
        new Todo(){Id = 1,OwnerId = 3,Label ="Label 1",IsDone = true,CreatedDateTime = new DateTime(2024, 2, 26),UpdatedDate = new DateTime(2024, 2, 27)},
        new Todo(){Id = 2,OwnerId = 2,Label ="Label 2",IsDone = false,CreatedDateTime = new DateTime(2024, 1, 25),UpdatedDate = new DateTime(2024, 1, 27)},
        new Todo(){Id = 3,OwnerId = 1,Label ="Label 3",IsDone = false,CreatedDateTime = new DateTime(2024, 1, 24),UpdatedDate = new DateTime(2024, 1, 26)},
        new Todo(){Id = 4,OwnerId = 3,Label ="Label 4",IsDone = true,CreatedDateTime = new DateTime(2024, 2, 23),UpdatedDate = new DateTime(2024, 2, 27)},
        new Todo(){Id = 5,OwnerId = 2,Label ="Label 5",IsDone = false,CreatedDateTime = new DateTime(2024, 1, 22),UpdatedDate = new DateTime(2024, 1, 27)},
        new Todo(){Id = 6,OwnerId = 1,Label ="Label 6",IsDone = false,CreatedDateTime = new DateTime(2024, 1, 21),UpdatedDate = new DateTime(2024, 1, 26)},
        new Todo(){Id = 7,OwnerId = 3,Label ="Label 7",IsDone = true,CreatedDateTime = new DateTime(2024, 2, 20),UpdatedDate = new DateTime(2024, 2, 27)},
        new Todo(){Id = 8,OwnerId = 2,Label ="Label 8",IsDone = false,CreatedDateTime = new DateTime(2024, 1, 28),UpdatedDate = new DateTime(2024, 1, 27)},
        new Todo(){Id = 9,OwnerId = 1,Label ="Label 9",IsDone = false,CreatedDateTime = new DateTime(2024, 1, 27),UpdatedDate = new DateTime(2024, 1, 26)},
        };

    public IReadOnlyCollection<Todo> GetList(int? offset, string? LabelFreeText, int? ownerId, int? limit = 10)
    {
        IEnumerable<Todo> todos = Todos;

        if (ownerId != null)
        {
            todos = todos.Where(b => b.OwnerId == ownerId);
        }

        if (!string.IsNullOrWhiteSpace(LabelFreeText))
        {
            todos = todos.Where(b => b.Label.Contains(LabelFreeText, StringComparison.InvariantCultureIgnoreCase));
        }

        todos = todos.OrderBy(t => t.Id);

        if (!limit.HasValue)
        {
            limit = 10;
        }

        if (offset.HasValue)
        {
            todos = todos.Skip(offset.Value);
        }

        todos = todos.Take(limit.Value).ToArray();
        return (IReadOnlyCollection<Todo>)todos;
    }

    public Todo? GetById(int id)
    {
        return Todos.SingleOrDefault(t => t.Id == id);
    }

    public Todo AddToList(Todo todo)
    {
        todo.Id = Todos.Max(todo => todo.Id) + 1;
        todo.CreatedDateTime = DateTime.UtcNow;
        todo.UpdatedDate = DateTime.UtcNow;
        Todos.Add(todo);
        return todo;
    }

    public Todo? PutToList(Todo todo)
    {
        todo.UpdatedDate = DateTime.UtcNow;
        return todo;
    }

    public Todo PatchIsDone(Todo todo)
    {
        var todoEntity = todo;
        return todoEntity;
    }
    public bool Remove(Todo todo)
    {
        var todoEntity = Todos.Single(t => t.Id == todo.Id);
        return Todos.Remove(todoEntity);
    }
}


