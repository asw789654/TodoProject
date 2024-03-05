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
        if (todos == null)
        {
            return NotFound();
        }
        var count = _todoService.Count(labelFreeText,ownerId);
        HttpContext.Response.Headers.Append("X-Tatal-Count",count.ToString());
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var todo = _todoService.GetById(id);

        if (todo == null)
        {
            return NotFound($"/{id}");
        }

        return Ok(todo);
    }

    [HttpGet("{id}/IsDone")]
    public IActionResult GetIsDoneById(int id)
    {
        var todo = _todoService.GetById(id);
        if (todo == null)
        {
            return NotFound($"/{id}");
        }
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
        if (_todoService.GetById(todo.Id) == null)
        {
            return NotFound();
        }
        return Ok(_todoService.Update(todo));
    }

    [HttpPatch]
    public IActionResult PatchIsDone(PatchIsDoneTodoDto todo)
    {
        if (_todoService.GetById(todo.Id) == null)
        {
            return NotFound();
        }
        return Ok(_todoService.PatchIsDone(todo));
    }

    [HttpDelete]
    public IActionResult Remove(RemoveTodoDto todo)
    {
        if (_todoService.GetById(todo.Id) == null)
        {
            return NotFound();
        }
        return Ok(_todoService.Delete(todo));
    }
}