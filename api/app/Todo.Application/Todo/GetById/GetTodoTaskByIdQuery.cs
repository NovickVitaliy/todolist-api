using Shared.CQRS.Queries;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.GetById;

public record GetTodoTaskByIdQuery(int Id) : IQuery<TodoTaskDto>;