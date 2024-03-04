using Todos.BL.DTO;
using Todos.Domain;

namespace Todos.BL;

public interface ITodoService
{
    IReadOnlyCollection<Todo> GetList(int? offset, string labelFreeText,int? ownerId, int? limit = 10);
    Todo? GetById(int id);
    public Todo? AddToList(CreateTodoDto todo);

    public Todo? Update(PutTodoDto todo);

    public Todo PatchIsDone(PatchIsDoneTodoDto todo);

    public int Count(string? labelFreeText);

    public bool Delete(RemoveTodoDto todo);

}
