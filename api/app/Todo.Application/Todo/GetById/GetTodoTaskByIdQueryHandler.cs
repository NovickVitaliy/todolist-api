using Shared.CQRS.Queries;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.GetById;

public class GetTodoTaskByIdQueryHandler : IQueryHandler<GetTodoTaskByIdQuery, Result<TodoTaskDto>>
{
    public Task<Result<TodoTaskDto>> Handle(GetTodoTaskByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}