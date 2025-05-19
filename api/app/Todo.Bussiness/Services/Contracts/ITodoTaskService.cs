using Shared;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;
using Todo.BussinessLayer.Dtos.Requests;

namespace Todo.BussinessLayer.Services.Contracts;

public interface ITodoTaskService
{
    Task<Result<TodoTaskDto>> CreateTodoTaskAsync(CreateTodoTaskRequest request);
    Task<Result<TodoTaskDto>> GetTodoTaskByIdAsync(int id);
    Task<Result<PagedResult<TodoTaskDto>>> GetTodoTasksPagedAsync(int pageNumber, int pageSize);
    Task<Result<TodoTaskDto>> UpdateTodoTaskAsync(int id, UpdateTodoTaskRequest request);
    Task<Result<bool>> DeleteTodoTaskAsync(int id);
}