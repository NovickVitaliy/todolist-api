using Shared.CQRS.Commands;

namespace Todo.Application.Todo.Delete;

public record DeleteTodoTaskCommand(int Id) : ICommand;