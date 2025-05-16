namespace Todo.Domain.Models;

public class TodoTask
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateOnly DueDate { get; set; }
    public Status Status { get; set; } = Status.Todo;
}