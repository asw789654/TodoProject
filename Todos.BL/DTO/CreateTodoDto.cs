﻿namespace Todos.BL.DTO;

public class CreateTodoDto
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public string? Label { get; set; }
    public bool IsDone { get; set; }
}
