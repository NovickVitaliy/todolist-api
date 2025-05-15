using Shared;
using Todo.Domain.Models;

namespace Todo.DataAccess.Repositories.Contracts;

public interface ITodoTaskRepository
{
    Task<TodoTask> CreateAsync(TodoTask todo);
    Task<TodoTask> UpdateAsync(TodoTask todo);
    Task<TodoTask?> GetByIdAsync(int id, bool isTracking);
    Task<PagedResult<TodoTask>> GetPagedAsync(int pageNumber, int pageSize, bool isTracking);
    Task DeleteAsync(int id);
}