using Todo.Domain.Models;

namespace Todo.BussinessLayer.Dtos;

public record UpdateTodoTaskRequest(
    string Name,
    string Description,
    DateOnly DueDate,
    Status Status);