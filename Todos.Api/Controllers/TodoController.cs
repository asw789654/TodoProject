using Microsoft.AspNetCore.Mvc;
using Todos.BL;
using Todos.BL.DTO;

namespace todoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;
    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public IActionResult GetList(int? limit, int? offset, int? ownerId, string? labelFreeText)
    {
        var todos = _todoService.GetList(offset, labelFreeText, ownerId, limit);
        var count = _todoService.Count(labelFreeText, ownerId);
        HttpContext.Response.Headers.Append("X-Tatal-Count", count.ToString());
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var todo = await _todoService.GetByIdAsync(id, cancellationToken);
        return Ok(todo);
    }

    [HttpGet("{id}/IsDone")]
    public IActionResult GetIsDoneById(int id)
    {
        var todo = _todoService.GetById(id);
        return Ok(todo.IsDone);
    }

    [HttpPost]
    public IActionResult AddToList(CreateTodoDto todo)
    {
        var todoEntity = _todoService.AddToList(todo);
        return Created($"/Todo/{todoEntity.Id}", todoEntity);
    }

    [HttpPut]
    public IActionResult PutToList(PutTodoDto todo)
    {
        return Ok(_todoService.Update(todo));
    }

    [HttpPatch]
    public IActionResult PatchIsDone(PatchIsDoneTodoDto todo)
    {
        return Ok(_todoService.PatchIsDone(todo));
    }

    [HttpDelete]
    public IActionResult Remove(RemoveTodoDto todo)
    {
        return Ok(_todoService.Delete(todo));
    }
}