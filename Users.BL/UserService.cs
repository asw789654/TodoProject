using AutoMapper;
using Common.Api.Exceptions;
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
    }

    public IReadOnlyCollection<User> GetList(int? offset, string nameFreeText, int? limit = 10)
    {
        return _usersRepository.GetList(offset, limit,
            nameFreeText == null ? null : u => u.Name.Contains(nameFreeText));
    }

    public User? GetById(int id)
    {
        var user = _usersRepository.SingleOrDefault(t => t.Id == id);
        if (user == null)
        {
            Log.Error($"Incorrect id -{user}");
            throw new NotFoundException(new { Id = id });
        }
        return _usersRepository.SingleOrDefault(u => u.Id == id);
    }

    public User? AddToList(AddUserDto addUserDto)
    {
        var userEntity = _mapper.Map<User>(addUserDto);
        userEntity.Id = _usersRepository.Count() + 1;
        return _usersRepository.Add(userEntity);
    }

    public User Update(UpdateUserDto user)
    {
        var userEntity = GetById(user.Id);
        _mapper.Map(user, userEntity);
        return _usersRepository.Update(userEntity);
    }
    public bool Delete(RemoveUserDto user)
    {
        var userEntity = GetById(user.Id);
        return _usersRepository.Delete(userEntity);
    }

}

