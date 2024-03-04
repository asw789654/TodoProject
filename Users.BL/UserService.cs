using Common.Domain;
using Common.Repositories;
using Users.BL.DTO;

namespace Users.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _usersRepository;

    public UserService(IRepository<User> usersRepository)
    {
        _usersRepository = usersRepository;
        _usersRepository.Add(new User() { Id = 1, Name = "name1" });
        _usersRepository.Add(new User() { Id = 2, Name = "name2" });
        _usersRepository.Add(new User() { Id = 3, Name = "name3" });
    }

    public IReadOnlyCollection<User> GetList(int? offset, string nameFreeText, int? limit = 10)
    {
        return _usersRepository.GetList(offset, limit,
            nameFreeText == null ? null : u => u.Name.Contains(nameFreeText));
    }

    public User? GetById(int id)
    {
        return _usersRepository.SingleOrDefault(u => u.Id == id);
    }

    public User? AddToList(AddUserDto user)
    {
        var userEntity = new User()
        {
            Name = user.name
        };
        return _usersRepository.Add(userEntity); ;
    }

    public User Update(User user)
    {
        var userEntity = new User()
        {
            Name = user.Name
        };
        return _usersRepository.Update(userEntity);
    }
    public bool Delete(RemoveUserDto user)
    {
        var userEntity = new User()
        {
            Id = user.Id
        };
        return _usersRepository.Delete(userEntity);
    }

}

