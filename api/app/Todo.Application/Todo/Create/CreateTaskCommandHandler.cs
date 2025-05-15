using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.Create;

public class CreateTaskCommandHandler : ICommandHandler<CreateTaskCommand, Result<TodoTaskDto>>
{
    public Task<Result<TodoTaskDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}