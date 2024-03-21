using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Users.BL.DTO;
using Users.Services;

namespace todoApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetList(int? limit, int? offset, string? labelFreeText, CancellationToken cancellationToken)
    {

        var users = await _userService.GetListAsync(offset, labelFreeText, limit, cancellationToken);
        return Ok(users);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(id, cancellationToken);

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> AddToListAsync(AddUserDto user, CancellationToken cancellationToken)
    {
        var userEntity = await _userService.AddToListAsync(user, cancellationToken);
        return Created($"/User/{userEntity.Id}", userEntity);
    }

    [HttpPut]
    public async Task<IActionResult> PutToListAsync(UpdateUserDto user, CancellationToken cancellationToken)
    {
        return Ok(await _userService.UpdateAsync(user, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveAsync(RemoveUserDto user, CancellationToken cancellationToken)
    {
        return Ok(await _userService.DeleteAsync(user, cancellationToken));
    }

    [HttpPut("{id}/Password")]
    public async Task<IActionResult> PutPasswordToListAsync(UpdateUserPasswordDto user, CancellationToken cancellationToken)
    {
        return Ok(await _userService.UpdatePasswordAsync(user, cancellationToken));
    }
}
