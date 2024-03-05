using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Users.BL.DTO;

namespace Users.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _usersRepository;
    private readonly IMapper _mapper;
    public UserService(IMapper mapper, IRepository<User> usersRepository)
    {
        _usersRepository = usersRepository;
        _mapper = mapper;
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
        var userEntity = _mapper.Map<AddUserDto,User>(user);
        return _usersRepository.Add(userEntity); 
    }

    public User Update(UpdateUserDto user)
    {
        var userEntity = _usersRepository.SingleOrDefault(t => t.Id == user.Id);
        if (userEntity == null)
        {
            return null;
        }
        
        _mapper.Map(user, userEntity);
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

