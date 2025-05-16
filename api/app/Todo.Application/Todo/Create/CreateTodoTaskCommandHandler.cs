using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;
using Todo.BussinessLayer.Services.Contracts;

namespace Todo.Application.Todo.Create;

public class CreateTodoTaskCommandHandler : ICommandHandler<CreateTodoTaskCommand, Result<TodoTaskDto>>
{
    private readonly ITodoTaskService _todoTaskService;
    
    public CreateTodoTaskCommandHandler(ITodoTaskService todoTaskService)
    {
        _todoTaskService = todoTaskService;
    }
    
    public async Task<Result<TodoTaskDto>> Handle(CreateTodoTaskCommand request, CancellationToken cancellationToken)
    {
        return await _todoTaskService.CreateTodoTaskAsync(request.Request);
    }
}