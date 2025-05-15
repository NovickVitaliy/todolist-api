namespace Todo.BussinessLayer.Dtos;

public record CreateTodoTaskRequest(
    string Name,
    string Description,
    DateOnly DueDate);