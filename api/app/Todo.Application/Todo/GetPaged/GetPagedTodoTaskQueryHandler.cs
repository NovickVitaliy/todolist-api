using Shared;
using Shared.CQRS.Queries;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;
using Todo.BussinessLayer.Services.Contracts;

namespace Todo.Application.Todo.GetPaged;

public class GetPagedTodoTaskQueryHandler : IQueryHandler<GetPagedTodoTaskQuery, Result<PagedResult<TodoTaskDto>>>
{
    private readonly ITodoTaskService _todoTaskService;
    
    public GetPagedTodoTaskQueryHandler(ITodoTaskService todoTaskService)
    {
        _todoTaskService = todoTaskService;
    }
    
    public async Task<Result<PagedResult<TodoTaskDto>>> Handle(GetPagedTodoTaskQuery request, CancellationToken cancellationToken)
    {
        return await _todoTaskService.GetTodoTasksPaged(request.PageNumber, request.PageSize);
    }
}