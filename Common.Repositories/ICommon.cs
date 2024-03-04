using Common.Domain;

namespace Common.Repositories
{
    public interface ICommon
    {
        public IReadOnlyCollection<User> GetList(int? offset, string? NameFreeText, int? limit = 10);
        public User? GetById(int id);
        public User? AddToList(User user);
        public User PutToList(User user);
        public bool Remove(User user);

    }
}
