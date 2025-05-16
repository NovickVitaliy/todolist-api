using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.Create;

public class CreateTodoTaskCommandHandler : ICommandHandler<CreateTodoTaskCommand, Result<TodoTaskDto>>
{
    public Task<Result<TodoTaskDto>> Handle(CreateTodoTaskCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}