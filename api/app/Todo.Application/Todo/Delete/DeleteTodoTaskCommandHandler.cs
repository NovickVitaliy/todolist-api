using MediatR;
using Shared.CQRS.Commands;

namespace Todo.Application.Todo.Delete;

public class DeleteTodoTaskCommandHandler : ICommandHandler<DeleteTodoTaskCommand>
{
    public Task<Unit> Handle(DeleteTodoTaskCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}