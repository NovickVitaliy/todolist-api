using Shared.CQRS.Commands;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.Create;

public record CreateTaskCommand(CreateTodoTaskRequest Request) : ICommand<TodoTaskDto>;