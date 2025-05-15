using Shared;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;

namespace Todo.BussinessLayer.Services.Contracts;

public interface ITodoTaskService
{
    Task<Result<TodoTaskDto>> CreateTodoTaskAsync(CreateTodoTaskRequest request);
    Task<Result<TodoTaskDto>> GetTodoTaskByIdAsync(int id);
    Task<Result<PagedResult<TodoTaskDto>>> GetTodoTasksPaged(int pageNumber, int pageSize);
    Task<Result<TodoTaskDto>> UpdateTodoTaskAsync(int id, UpdateTodoTaskRequest request);
    Task DeleteTodoTaskAsync(int id);
}