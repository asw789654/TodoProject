using Common.Domain;

namespace Common.Repositories
{
    public class Common : ICommon
    {
        public static List<User> Users = new List<User>(){
        new User(){Id = 1,Name = "name1"},
        new User(){Id = 2,Name = "name2"},
        new User(){Id = 3,Name = "name3"},
        };

        public IReadOnlyCollection<User> GetList(int? offset, string? NameFreeText, int? limit = 10)
        {
            IEnumerable<User> users = Users;

            if (!string.IsNullOrWhiteSpace(NameFreeText))
            {
                users = users.Where(b => b.Name.Contains(NameFreeText, StringComparison.InvariantCultureIgnoreCase));
            }

            users = users.OrderBy(t => t.Id);

            if (!limit.HasValue)
            {
                limit = 10;
            }

            if (offset.HasValue)
            {
                users = users.Skip(offset.Value);
            }

            users = users.Take(limit.Value).ToArray();
            return (IReadOnlyCollection<User>)users;
        }

        public User? GetById(int id)
        {
            return Users.SingleOrDefault(u => u.Id == id);
        }

        public User? AddToList(User user)
        {
            user.Id = Users.Max(user => user.Id) + 1;
            Users.Add(user);
            return user;
        }

        public User PutToList(User user)
        {
            return user;
        }

        public bool Remove(User user)
        {
            var userEntity = Users.Single(u => u.Id == user.Id);
            return Users.Remove(userEntity);
        }
    }
}
