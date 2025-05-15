using Shared.CQRS.Queries;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.GetById;

public class GetTodoTaskByIdQueryHandler : IQueryHandler<GetTodoTaskByIdQuery, TodoTaskDto>
{
    public Task<TodoTaskDto> Handle(GetTodoTaskByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}