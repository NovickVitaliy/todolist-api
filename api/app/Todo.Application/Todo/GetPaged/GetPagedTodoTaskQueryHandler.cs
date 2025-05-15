using Shared;
using Shared.CQRS.Queries;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.GetPaged;

public class GetPagedTodoTaskQueryHandler : IQueryHandler<GetPagedTodoTaskQuery, Result<PagedResult<TodoTaskDto>>>
{
    public Task<Result<PagedResult<TodoTaskDto>>> Handle(GetPagedTodoTaskQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}