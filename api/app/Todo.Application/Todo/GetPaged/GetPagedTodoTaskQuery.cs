using Shared;
using Shared.CQRS.Queries;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.GetPaged;

public record GetPagedTodoTaskQuery(int PageNumber, int PageSize) : IQuery<PagedResult<TodoTaskDto>>;