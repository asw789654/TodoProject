using Auth.BL.Utils;
using AutoMapper;
using Common.Api.Exceptions;
using Common.Api.Services;
using Common.Domain;
using Common.Repositories;
using Serilog;
using Users.BL.DTO;

namespace Users.Services;

public class UserService : IUserService
{
    private readonly IRepository<ApplicationUser> _usersRepository;
    private readonly IRepository<ApplicationUserRole> _userRoles;
    private readonly CurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    
    public UserService(
        CurrentUserService currentUserService,
        IRepository<ApplicationUser> usersRepository, 
        IRepository<ApplicationUserRole> userRoles,
        IMapper mapper)
    {
        _currentUserService = currentUserService;
        _usersRepository = usersRepository;
        _userRoles = userRoles;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<GetUserDto>> GetListAsync(
        int? offset,
        string nameFreeText,
        int? limit = 10,
        CancellationToken cancellationToken = default)
    {
        return _mapper.Map<IReadOnlyCollection<GetUserDto>>( await _usersRepository.GetListAsync(
            offset,
            limit,
            nameFreeText == null ? null : u => u.Name.Contains(nameFreeText, StringComparison.InvariantCulture),
            null,
            u => u.Id,
            false,
            cancellationToken));
    }

    public async Task<GetUserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = _mapper.Map<GetUserDto>(await _usersRepository.SingleOrDefaultAsync(t => t.Id == id, cancellationToken));
        if (user == null)
        {
            Log.Error($"Incorrect id -{user}");
            throw new NotFoundException(new { Id = id });
        }
        return user;
    }

    public async Task<GetUserDto?> AddToListAsync(AddUserDto addUserDto, CancellationToken cancellationToken = default)
    {
        if(await _usersRepository.SingleOrDefaultAsync(u => u.Name == addUserDto.Name.Trim()) is not null)
        {
            throw new BadRequestException("User login exists");
        }
        var userRole = (await _userRoles.SingleOrDefaultAsync(r => r.Name == "Admin",cancellationToken))!;
        //var userEntity = _mapper.Map<User>(addUserDto);
        var userEntity = new ApplicationUser()
        {
            Name = addUserDto.Name,
            PasswordHash = PasswordHasher.HashPassword(addUserDto.Password),
            Roles = new[] { new ApplicationUserApplicationRole() {ApplicationUserRoleId = userRole.Id } }
        };
        userEntity.Id = _usersRepository.CountAsync(cancellationToken: cancellationToken).Result + 1;
        return _mapper.Map<GetUserDto>(await _usersRepository.AddAsync(userEntity, cancellationToken));
    }

    public async Task<GetUserDto> UpdateAsync(UpdateUserDto user, CancellationToken cancellationToken = default)
    {
        GetByIdAsync(user.Id, cancellationToken);
        var userEntity = new ApplicationUser()
        {
            Id = user.Id,
            Name = user.Name,
            PasswordHash = PasswordHasher.HashPassword(user.Password)
        };
        _mapper.Map(user, userEntity);
        if (_currentUserService.CurrentUserRoles().Contains("Admin"))
        {
            return _mapper.Map<GetUserDto>(await _usersRepository.UpdateAsync(userEntity, cancellationToken));
        }
        else if (_currentUserService.CurrentUserId() == _usersRepository.SingleOrDefaultAsync(e => e.Id == user.Id).Result.Id)
        {
            return _mapper.Map<GetUserDto>(await _usersRepository.UpdateAsync(userEntity, cancellationToken));
        }
        else
        {
            throw new BadRequestException("Access denied");
        }
        
    }
    public async Task<GetUserDto> UpdatePasswordAsync(UpdateUserPasswordDto user, CancellationToken cancellationToken = default)
    {
        GetByIdAsync(user.Id, cancellationToken);
        var userEntity = new ApplicationUser()
        {
            Id = user.Id,
            PasswordHash = PasswordHasher.HashPassword(user.Password)
        };
        _mapper.Map(user, userEntity);
        if (_currentUserService.CurrentUserRoles().Contains("Admin"))
        {
            return _mapper.Map<GetUserDto>(await _usersRepository.UpdateAsync(userEntity, cancellationToken));
        }
        else if (_currentUserService.CurrentUserId() == _usersRepository.SingleOrDefaultAsync(e => e.Id == user.Id).Result.Id)
        {
            return _mapper.Map<GetUserDto>(await _usersRepository.UpdateAsync(userEntity, cancellationToken));
        }
        else
        {
            throw new BadRequestException("Access denied");
        }

    }
    public async Task<bool> DeleteAsync(RemoveUserDto user, CancellationToken cancellationToken = default)
    {
        GetByIdAsync(user.Id, cancellationToken);
        var userEntity = new ApplicationUser()
        {
            Id = user.Id,
        };
        if (_currentUserService.CurrentUserRoles().Contains("Admin"))
        {
            return await _usersRepository.DeleteAsync(userEntity, cancellationToken);
        }
        else if (_currentUserService.CurrentUserId() == _usersRepository.SingleOrDefaultAsync(e => e.Id == user.Id).Result.Id)
        {
            return await _usersRepository.DeleteAsync(userEntity, cancellationToken);
        }
        else
        {
            throw new BadRequestException("Access denied");
        }        
    }

}

