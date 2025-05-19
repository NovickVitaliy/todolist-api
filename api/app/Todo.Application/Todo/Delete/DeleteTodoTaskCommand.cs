using Shared.CQRS.Commands;
using Shared.ErrorHandling;

namespace Todo.Application.Todo.Delete;

public record DeleteTodoTaskCommand(int Id) : ICommand<Result<bool>>;