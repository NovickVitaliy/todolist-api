using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.Update;

public class UpdateTodoTaskCommandHandler : ICommandHandler<UpdateTodoTaskCommand, Result<TodoTaskDto>>
{
    public Task<Result<TodoTaskDto>> Handle(UpdateTodoTaskCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}