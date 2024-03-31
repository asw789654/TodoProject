using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todos.Application.Query.GetById;
using Todos.Application.Query.GetCount;
using Todos.Application.Query.GetList;
using Todos.Application.Commands.AddToList;
using Todos.Application.Commands.Delete;
using Todos.Application.Commands.PatchIsDone;
using Todos.Application.Commands.Update;
using MediatR;


namespace Todo.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetList(
        [AsParameters] GetTodoListQuery getTodoListQuery,
       // GetTodoCountQuery getTodoCountQuery,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var todos = await mediator.Send(getTodoListQuery, cancellationToken);
        //var count = await mediator.Send(getTodoCountQuery, cancellationToken);
        //HttpContext.Response.Headers.Append("X-Tatal-Count", count.ToString());
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromQuery] GetTodoByIdQuery getTodoByIdQuery,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var todo = await mediator.Send(getTodoByIdQuery, cancellationToken);
        return Ok(todo);

    }

    [HttpGet("{id}/IsDone")]
    public async Task<IActionResult> GetIsDoneByIdAsync(
        [FromQuery] GetTodoByIdQuery getTodoByIdQuery,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var todo = await mediator.Send(getTodoByIdQuery, cancellationToken);
        return Ok(todo.IsDone);
    }
    [HttpPost]
    public async Task<IActionResult> AddToListAsync(
        [FromBody] AddTodoToListCommand todo, 
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var todoEntity = await mediator.Send(todo, cancellationToken);
        return Created($"/Todo/{todoEntity.Id}", todoEntity); // todoEntity.Id
    }

    [HttpPut]
    public async Task<IActionResult> PutToListAsync(
        [FromBody] UpdateTodoCommand todo, 
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(todo, cancellationToken));
    }

    [HttpPatch]
    public async Task<IActionResult> PatchIsDoneAsync(
        [FromBody] PatchTodoIsDoneCommand todo, 
        IMediator mediator, 
        CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(todo, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(
        [FromBody] DeleteTodoCommand todo, 
        IMediator mediator, 
        CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(todo, cancellationToken));
    }
}