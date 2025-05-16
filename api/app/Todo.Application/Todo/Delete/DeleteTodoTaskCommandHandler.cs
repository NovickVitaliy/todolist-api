using MediatR;
using Shared.CQRS.Commands;
using Todo.BussinessLayer.Services.Contracts;

namespace Todo.Application.Todo.Delete;

public class DeleteTodoTaskCommandHandler : ICommandHandler<DeleteTodoTaskCommand>
{
    private readonly ITodoTaskService _todoTaskService;
    
    public DeleteTodoTaskCommandHandler(ITodoTaskService todoTaskService)
    {
        _todoTaskService = todoTaskService;
    }
    public async Task<Unit> Handle(DeleteTodoTaskCommand request, CancellationToken cancellationToken)
    {
        await _todoTaskService.DeleteTodoTaskAsync(request.Id);
        return Unit.Value;
    }
}