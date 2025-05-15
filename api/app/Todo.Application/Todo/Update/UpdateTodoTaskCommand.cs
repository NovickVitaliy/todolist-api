using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;
using Todo.BussinessLayer.Dtos.Requests;

namespace Todo.Application.Todo.Update;

public record UpdateTodoTaskCommand(int Id, UpdateTodoTaskRequest Request) : ICommand<Result<TodoTaskDto>>;