using Todos.Domain;

namespace Todos.Repositories
{
    public interface ITodoRepository
    {
        IReadOnlyCollection<Todo> GetList(int? offset, string? labelFreeText, int? ownerId, int? limit);
        Todo? GetById(int id);
        public Todo? AddToList(Todo todo);

        public Todo? PutToList(Todo todo);

        public Todo PatchIsDone(Todo todo);

        public bool Remove(Todo todo);
    }
}
