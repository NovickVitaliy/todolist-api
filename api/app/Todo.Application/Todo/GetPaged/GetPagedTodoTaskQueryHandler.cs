using Shared;
using Shared.CQRS.Queries;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.GetPaged;

public class GetPagedTodoTaskQueryHandler : IQueryHandler<GetPagedTodoTaskQuery, PagedResult<TodoTaskDto>>
{
    public Task<PagedResult<TodoTaskDto>> Handle(GetPagedTodoTaskQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}