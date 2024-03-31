using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Application.Commands.AddUser;
using Users.Application.Commands.Delete;
using Users.Application.Commands.Update;
using Users.Application.Commands.UpdatePassword;
using Users.Application.Query.GetById;
using Users.Application.Query.GetList;

namespace Users.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery]GetUserListQuery getUserlistQuery,
        IMediator mediator,
        CancellationToken cancellationToken)
    {

        var users = await mediator.Send(getUserlistQuery, cancellationToken);
        return Ok(users);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromQuery] GetUserByIdQuery getUserByIdQuery,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var user = await mediator.Send(getUserByIdQuery, cancellationToken);

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> AddToListAsync(
        [FromBody] AddUserCommand user,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var userEntity = await mediator.Send(user, cancellationToken);
        return Created($"/User/{userEntity.Id}", userEntity);
    }

    [HttpPut]
    public async Task<IActionResult> PutToListAsync(
        [FromBody] UpdateUserCommand user,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(user, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveAsync(
        [FromBody] DeleteUserCommand user,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(user, cancellationToken));
    }

    [HttpPut("{id}/Password")]
    public async Task<IActionResult> PutPasswordToListAsync(
        [FromBody] UpdateUserPasswordCommand user,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(user, cancellationToken));
    }
}
