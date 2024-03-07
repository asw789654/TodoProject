using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Serilog;
using Users.BL.DTO;

namespace Users.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _usersRepository;
    private readonly IMapper _mapper;
    public UserService(IMapper mapper,
        IRepository<User> usersRepository)
    {
        _usersRepository = usersRepository;
        _mapper = mapper;
        if (_usersRepository.Count() == 0)
        {
            _usersRepository.Add(new User() { Id = 1, Name = "name1" });
            _usersRepository.Add(new User() { Id = 2, Name = "name2" });
            _usersRepository.Add(new User() { Id = 3, Name = "name3" });
        }
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

    public User? AddToList(AddUserDto addUserDto)
    {
        var userEntity = _mapper.Map<User>(addUserDto);
        return _usersRepository.Add(userEntity);
    }

    public User Update(UpdateUserDto user)
    {
        var userEntity = GetById(user.Id);
        if (userEntity == null)
        {
            Log.Error($"Incorrect user id -{user.Id}");
            throw new Exception("Incorrect User");
        }

        _mapper.Map(user, userEntity);
        return _usersRepository.Update(userEntity);
    }
    public bool Delete(RemoveUserDto user)
    {
        var userEntity = GetById(user.Id);
        if (userEntity is null)
        {
            Log.Error($"Incorrect user id -{user.Id}");
            throw new Exception("Incorrect User");
        }
        return _usersRepository.Delete(userEntity);
    }

}

