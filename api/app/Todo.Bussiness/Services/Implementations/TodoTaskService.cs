using Mapster;
using MapsterMapper;
using Shared;
using Shared.BaseRequest;
using Shared.ErrorHandling;
using Todo.BussinessLayer.Dtos;
using Todo.BussinessLayer.Dtos.Requests;
using Todo.BussinessLayer.Services.Contracts;
using Todo.DataAccess.Repositories.Contracts;
using Todo.Domain.Models;

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
    
    public async Task<Result<TodoTaskDto>> CreateTodoTaskAsync(CreateTodoTaskRequest request)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
        {
            return Result<TodoTaskDto>.BadRequest(validationResult.ErrorMessage);
        }

        var todoTask = request.Adapt<TodoTask>();

        todoTask = await _todoTaskRepository.CreateAsync(todoTask);

        var dto = todoTask.Adapt<TodoTaskDto>();
        
        return Result<TodoTaskDto>.Created($"/api/todos/{dto.Id}", dto);
    }
    
    public async Task<Result<TodoTaskDto>> GetTodoTaskByIdAsync(int id)
    {
        var todoTask = await _todoTaskRepository.GetByIdAsync(id, false);
        if (todoTask is null)
        {
            return Result<TodoTaskDto>.NotFound(id);
        }

        var dto = todoTask.Adapt<TodoTaskDto>();
        
        return Result<TodoTaskDto>.Ok(dto);
    }
    
    public async Task<Result<PagedResult<TodoTaskDto>>> GetTodoTasksPagedAsync(int pageNumber, int pageSize)
    {
        var validationResult = ValidatePagedRequest(pageNumber, pageSize);
        if (!validationResult.IsValid)
        {
            return Result<PagedResult<TodoTaskDto>>.BadRequest(validationResult.ErrorMessage);
        }

        var data = await _todoTaskRepository.GetPagedAsync(pageNumber, pageSize, false);

        var result = new PagedResult<TodoTaskDto>(
                data.Items.Select(x => x.Adapt<TodoTaskDto>()).ToArray(),
                data.CurrentPage,
                data.ItemsPerPage,
                data.TotalPages,
                data.TotalItemsCount);
        
        return Result<PagedResult<TodoTaskDto>>.Ok(result);
    }
    
    private ValidationResult ValidatePagedRequest(int pageNumber, int pageSize)
    {
        List<string> errors = [];
        if (pageNumber <= 0)
        {
            errors.Add("Page Number cannot be less than or equal to zero");
        }

        if (pageSize <= 0)
        {
            errors.Add("Page Size cannot be less than or equal to zero");
        }

        return errors.Count == 0
            ? new ValidationResult(true)
            : new ValidationResult(false, errors[0]);
    }

    public async Task<Result<TodoTaskDto>> UpdateTodoTaskAsync(int id, UpdateTodoTaskRequest request)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
        {
            return Result<TodoTaskDto>.BadRequest(validationResult.ErrorMessage); 
        }

        var todo = await _todoTaskRepository.GetByIdAsync(id, true);
        if (todo is null)
        {
            return Result<TodoTaskDto>.NotFound(id);
        }

        todo = request.Adapt(todo);

        await _todoTaskRepository.UpdateAsync(todo);
        
        return Result<TodoTaskDto>.Ok(todo.Adapt<TodoTaskDto>());
    }
    
    public async Task<Result<bool>> DeleteTodoTaskAsync(int id)
    {
        var todoTask = await _todoTaskRepository.GetByIdAsync(id, false);
        if (todoTask is null)
        {
            return Result<bool>.NotFound(id);
        }
        
        await _todoTaskRepository.DeleteAsync(id);
        return Result<bool>.NoContent();
    }
}