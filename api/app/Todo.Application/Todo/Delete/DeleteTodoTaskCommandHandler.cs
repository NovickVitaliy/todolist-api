using MediatR;
using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Services.Contracts;

namespace Todo.Application.Todo.Delete;

public class DeleteTodoTaskCommandHandler : ICommandHandler<DeleteTodoTaskCommand, Result<bool>>
{
    private readonly ITodoTaskService _todoTaskService;
    
    public DeleteTodoTaskCommandHandler(ITodoTaskService todoTaskService)
    {
        _todoTaskService = todoTaskService;
    }
    public async Task<Result<bool>> Handle(DeleteTodoTaskCommand request, CancellationToken cancellationToken)
    {
        return await _todoTaskService.DeleteTodoTaskAsync(request.Id);
    }
}