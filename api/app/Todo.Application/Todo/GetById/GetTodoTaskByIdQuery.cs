using Shared.CQRS.Queries;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;

namespace Todo.Application.Todo.GetById;

public record GetTodoTaskByIdQuery(int Id) : IQuery<Result<TodoTaskDto?>>;