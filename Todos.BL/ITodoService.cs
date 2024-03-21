using Common.Domain;
using Todos.BL.DTO;

namespace Todos.BL;

public interface ITodoService
{
    public Task<IReadOnlyCollection<Todo>> GetListAsync(
        int? offset,
        string labelFreeText,
        int? ownerId,
        int? limit = 10,
        CancellationToken cancellationToken = default);
    public Task<Todo?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<Todo?> AddToListAsync(CreateTodoDto todo, CancellationToken cancellationToken = default);

    public Task<Todo?> UpdateAsync(PutTodoDto todo, CancellationToken cancellationToken = default);

    public Task<Todo> PatchIsDoneAsync(PatchIsDoneTodoDto todo, CancellationToken cancellationToken = default);

    public Task<int> CountAsync(string? labelFreeText, int? OwnerId, CancellationToken cancellationToken = default);

    public Task<bool> DeleteAsync(RemoveTodoDto todo, CancellationToken cancellationToken = default);

}
