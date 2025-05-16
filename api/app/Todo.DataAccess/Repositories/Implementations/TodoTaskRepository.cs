using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Shared;
using Todo.DataAccess.Database;
using Todo.DataAccess.Repositories.Contracts;
using Todo.Domain.Models;

namespace Todo.DataAccess.Repositories.Implementations;

public class TodoTaskRepository : ITodoTaskRepository
{
    private TodoDbContext _dbContext;

    public TodoTaskRepository(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TodoTask> CreateAsync(TodoTask todo)
    {
        _dbContext.TodoTasks.Add(todo);
        await _dbContext.SaveChangesAsync();
        return todo;
    }
    
    public async Task<TodoTask> UpdateAsync(TodoTask todo)
    {
        _dbContext.TodoTasks.Update(todo); 
        await _dbContext.SaveChangesAsync();
        return todo;
    }
    
    public async Task<TodoTask?> GetByIdAsync(int id, bool isTracking)
    {
        var query = _dbContext.TodoTasks.Where(x => x.Id == id);

        var todo = isTracking
            ? await query.SingleOrDefaultAsync()
            : await query.AsNoTracking().SingleOrDefaultAsync();

        return todo;
    }
    
    public async Task<PagedResult<TodoTask>> GetPagedAsync(int pageNumber, int pageSize, bool isTracking)
    {
        var query = _dbContext.TodoTasks
            .OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var todos = isTracking
            ? await query.ToArrayAsync()
            : await query.AsNoTracking().ToArrayAsync();

        var totalCount = await _dbContext.TodoTasks.CountAsync();
        
        return new PagedResult<TodoTask>(todos, pageNumber, pageSize, Math.Ceiling((double)totalCount / pageSize));
    }
    
    public async Task DeleteAsync(int id)
    {
        var todo = await _dbContext.TodoTasks.SingleOrDefaultAsync(x => x.Id == id);

        _dbContext.Remove(todo);

        await _dbContext.SaveChangesAsync();
    }
}