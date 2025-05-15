using MapsterMapper;
using Shared;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;
using Todo.BussinessLayer.Dtos.Requests;
using Todo.BussinessLayer.Services.Contracts;
using Todo.DataAccess.Repositories.Contracts;

namespace Todo.BussinessLayer.Services.Implementations;

public class TodoTaskService : ITodoTaskService
{
    private readonly ITodoTaskRepository _todoTaskRepository;
    private readonly IMapper _mapper;
    
    public TodoTaskService(ITodoTaskRepository todoTaskRepository, IMapper mapper)
    {
        _todoTaskRepository = todoTaskRepository;
        _mapper = mapper;
    }
    
    public Task<Result<TodoTaskDto>> CreateTodoTaskAsync(CreateTodoTaskRequest request)
    {
        throw new NotImplementedException();
    }
    
    public Task<Result<TodoTaskDto>> GetTodoTaskByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
    
    public Task<Result<PagedResult<TodoTaskDto>>> GetTodoTasksPaged(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }
    
    public Task<Result<TodoTaskDto>> UpdateTodoTaskAsync(int id, UpdateTodoTaskRequest request)
    {
        throw new NotImplementedException();
    }
    
    public Task DeleteTodoTaskAsync(int id)
    {
        throw new NotImplementedException();
    }
}