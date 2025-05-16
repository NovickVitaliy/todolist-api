using Shared.CQRS.Queries;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;
using Todo.BussinessLayer.Services.Contracts;

namespace Todo.Application.Todo.GetById;

public class GetTodoTaskByIdQueryHandler : IQueryHandler<GetTodoTaskByIdQuery, Result<TodoTaskDto>>
{
    private readonly ITodoTaskService _todoTaskService;
    
    public GetTodoTaskByIdQueryHandler(ITodoTaskService todoTaskService)
    {
        _todoTaskService = todoTaskService;
    }
    
    public async Task<Result<TodoTaskDto>> Handle(GetTodoTaskByIdQuery request, CancellationToken cancellationToken)
    {
        return await _todoTaskService.GetTodoTaskByIdAsync(request.Id);
    }
}