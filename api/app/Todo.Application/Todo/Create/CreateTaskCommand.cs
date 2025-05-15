using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;
using Todo.BussinessLayer.Dtos.Requests;

namespace Todo.Application.Todo.Create;

public record CreateTaskCommand(CreateTodoTaskRequest Request) : ICommand<Result<TodoTaskDto>>;