using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;
using Todo.BussinessLayer.Services.Contracts;

namespace Todo.Application.Todo.Update;

public class UpdateTodoTaskCommandHandler : ICommandHandler<UpdateTodoTaskCommand, Result<TodoTaskDto>>
{
    private readonly ITodoTaskService _todoTaskService;
    
    public UpdateTodoTaskCommandHandler(ITodoTaskService todoTaskService)
    {
        _todoTaskService = todoTaskService;
    }
    
    public async Task<Result<TodoTaskDto>> Handle(UpdateTodoTaskCommand request, CancellationToken cancellationToken)
    {
        return await _todoTaskService.UpdateTodoTaskAsync(request.Id, request.Request);
    }
}