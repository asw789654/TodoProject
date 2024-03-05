﻿namespace Todos.Domain
{
    public class Todo
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Label { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
