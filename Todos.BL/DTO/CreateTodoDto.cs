namespace Todos.BL.DTO;

public class CreateTodoDto
{
    public int OwnerId { get; set; }
    public string? Label { get; set; } = default;
    public bool IsDone { get; set; }
}
