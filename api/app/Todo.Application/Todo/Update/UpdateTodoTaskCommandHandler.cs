using Shared.CQRS.Commands;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.Update;

public class UpdateTodoTaskCommandHandler : ICommandHandler<UpdateTodoTaskCommand, TodoTaskDto>
{
    public Task<TodoTaskDto> Handle(UpdateTodoTaskCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}