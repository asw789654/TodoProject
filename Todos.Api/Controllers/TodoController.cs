using Common.Api.Exceptions;
using Common.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todos.BL;
using Todos.BL.DTO;

namespace todoApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;
    private readonly ICurrentUserService _currentUserService;

    public TodoController(ITodoService todoService, ICurrentUserService currentUserService)
    {
        _todoService = todoService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetList(int? limit, int? offset, int? ownerId, string? labelFreeText, CancellationToken cancellationToken)
    {
        var todos = await _todoService.GetListAsync(offset, labelFreeText, ownerId, limit, cancellationToken);
        var count = await _todoService.CountAsync(labelFreeText, ownerId, cancellationToken);
        HttpContext.Response.Headers.Append("X-Tatal-Count", count.ToString());
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var todo = await _todoService.GetByIdAsync(id, cancellationToken);
        return Ok(todo);

    }

    [HttpGet("{id}/IsDone")]
    public async Task<IActionResult> GetIsDoneByIdAsync(int id, CancellationToken cancellationToken)
    {
        var todo = await _todoService.GetByIdAsync(id, cancellationToken);
        return Ok(todo);
    }
    [HttpPost]
    public async Task<IActionResult> AddToListAsync(CreateTodoDto todo, CancellationToken cancellationToken)
    {
        var todoEntity = await _todoService.AddToListAsync(todo, cancellationToken);
        return Created($"/Todo/{todoEntity.Id}", todoEntity);
    }

    [HttpPut]
    public async Task<IActionResult> PutToListAsync(PutTodoDto todo, CancellationToken cancellationToken)
    {
        return Ok(await _todoService.UpdateAsync(todo, cancellationToken));
    }

    [HttpPatch]
    public async Task<IActionResult> PatchIsDoneAsync(PatchIsDoneTodoDto todo, CancellationToken cancellationToken)
    {
        return Ok(await _todoService.PatchIsDoneAsync(todo, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveAsync(RemoveTodoDto todo, CancellationToken cancellationToken)
    {
        return Ok(await _todoService.DeleteAsync(todo, cancellationToken));
    }
}