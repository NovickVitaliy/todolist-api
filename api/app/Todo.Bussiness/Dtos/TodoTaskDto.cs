using Todo.Domain.Models;

namespace Todo.BussinessLayer.Dtos;

public record TodoTaskDto(int Id, string Name, string Description, DateOnly DueDate, Status Status);