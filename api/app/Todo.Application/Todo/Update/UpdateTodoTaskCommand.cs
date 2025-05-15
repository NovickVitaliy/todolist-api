using Shared.CQRS.Commands;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.Update;

public record UpdateTodoTaskCommand(int Id, UpdateTodoTaskRequest Request) : ICommand<TodoTaskDto>;