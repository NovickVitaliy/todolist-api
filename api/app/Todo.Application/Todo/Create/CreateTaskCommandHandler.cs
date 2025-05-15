using Shared.CQRS.Commands;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.Create;

public class CreateTaskCommandHandler : ICommandHandler<CreateTaskCommand, TodoTaskDto>
{
    public Task<TodoTaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}