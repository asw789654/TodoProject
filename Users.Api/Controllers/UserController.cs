using Common.Domain;
using Microsoft.AspNetCore.Mvc;
using Users.BL.DTO;
using Users.Services;

namespace todoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetList(int? limit, int? offset, string? labelFreeText)
    {
        var users = _userService.GetList(offset, labelFreeText, limit);
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = _userService.GetById(id);

        return Ok(user);
    }

    [HttpPost]
    public IActionResult AddToList(AddUserDto user)
    {
        var userEntity = _userService.AddToList(user);
        return Created($"/User/{userEntity.Id}", userEntity);
    }

    [HttpPut]
    public IActionResult PutToList(UpdateUserDto user)
    {
        return Ok(_userService.Update(user));
    }

    [HttpDelete]
    public IActionResult Remove(RemoveUserDto user)
    {
        return Ok(_userService.Delete(user));
    }
}
