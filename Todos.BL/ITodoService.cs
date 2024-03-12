using Common.Domain;
using System.Threading;
using Todos.BL.DTO;

namespace Todos.BL;

public interface ITodoService
{
    IReadOnlyCollection<Todo> GetList(int? offset, string labelFreeText, int? ownerId, int? limit = 10);
    Task<Todo?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Todo GetById(int id);
    public Todo? AddToList(CreateTodoDto todo);

    public Todo? Update(PutTodoDto todo);

    public Todo PatchIsDone(PatchIsDoneTodoDto todo);

    public int Count(string? labelFreeText, int? OwnerId);

    public bool Delete(RemoveTodoDto todo);

}
