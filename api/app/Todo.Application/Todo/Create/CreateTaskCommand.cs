using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.Create;

public record CreateTaskCommand(CreateTodoTaskRequest Request) : ICommand<Result<TodoTaskDto>>;