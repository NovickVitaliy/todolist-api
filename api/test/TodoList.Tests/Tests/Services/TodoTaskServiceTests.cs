using System.Net;
using MapsterMapper;
using Moq;
using Shared;
using Todo.BussinessLayer.Dtos;
using Todo.BussinessLayer.Dtos.Requests;
using Todo.BussinessLayer.Services.Implementations;
using Todo.DataAccess.Repositories.Contracts;
using Todo.Domain.Models;

namespace TodoList.Tests.Tests.Services;

public class TodoTaskServiceTests
{
    private readonly Mock<ITodoTaskRepository> _mockTodoTaskRepository;

    public TodoTaskServiceTests()
    {
        _mockTodoTaskRepository = new Mock<ITodoTaskRepository>();
    }

    [Fact]
    public async Task CreateTodoTaskAsync_GivenValidRequest_CreatesTodoTask()
    {
        var request = new CreateTodoTaskRequest("First", "Test", DateOnly.MaxValue);
        var todo = new TodoTask()
        {
            Id = 1,
            DueDate = DateOnly.MaxValue,
            Name = "First",
            Status = Status.Todo,
            Description = "Test"
        };
        
        var todoTaskDto = new TodoTaskDto(1, "First", "Test", DateOnly.MaxValue, Status.Todo);
        
        _mockTodoTaskRepository.Setup(x => x.CreateAsync(It.IsAny<TodoTask>()))
            .ReturnsAsync(todo);
        
        var todoTaskService = new TodoTaskService(_mockTodoTaskRepository.Object, new Mapper());
        
        var result = await todoTaskService.CreateTodoTaskAsync(request);
        
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Description);
        Assert.Equal("/api/todos/1", result.Location!);
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        Assert.Equal(todoTaskDto.Id, result.Data!.Id);
        Assert.Equal(todoTaskDto.Name, result.Data!.Name);
        Assert.Equal(todoTaskDto.Description, result.Data!.Description);
        Assert.Equal(todoTaskDto.DueDate, result.Data!.DueDate);
        Assert.Equal(todoTaskDto.Status, result.Data!.Status);
    }

    [Fact]
    public async Task CreateTodoTaskAsync_GivenRequestWithInvalidName_ReturnsFailure()
    {
        var request = new CreateTodoTaskRequest(string.Empty, "123", DateOnly.MaxValue);
        var service = new TodoTaskService(_mockTodoTaskRepository.Object, new Mapper());

        var result = await service.CreateTodoTaskAsync(request);
        
        Assert.False(result.Success);
        Assert.Equal("Name cannot be empty", result.Description);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Null(result.Data);
    }
    
    [Fact]
    public async Task CreateTodoTaskAsync_GivenRequestWithInvalidDueDate_ReturnsFailure()
    {
        var request = new CreateTodoTaskRequest("123", "123", DateOnly.MinValue);
        var service = new TodoTaskService(_mockTodoTaskRepository.Object, new Mapper());

        var result = await service.CreateTodoTaskAsync(request);
        
        Assert.False(result.Success);
        Assert.Equal("Due Date cannot be past", result.Description);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task GetTodoTaskByIdAsync_GivenExistingId_ReturnsRecord()
    {
        const int id = 1;
        var todo = new TodoTask()
        {
            Id = 1,
            DueDate = DateOnly.MaxValue,
            Name = "First",
            Status = Status.Todo,
            Description = "123"
        };

        var expectedDto = new TodoTaskDto(1, "First", "123", DateOnly.MaxValue, Status.Todo);
        
        _mockTodoTaskRepository.Setup(x => x.GetByIdAsync(1, false)).ReturnsAsync(todo);

        var service = new TodoTaskService(_mockTodoTaskRepository.Object, new Mapper());

        var result = await service.GetTodoTaskByIdAsync(id);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Description);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(expectedDto.Id, result.Data!.Id);
        Assert.Equal(expectedDto.Name, result.Data!.Name);
        Assert.Equal(expectedDto.Description, result.Data!.Description);
        Assert.Equal(expectedDto.DueDate, result.Data!.DueDate);
        Assert.Equal(expectedDto.Status, result.Data!.Status);
        _mockTodoTaskRepository.Verify(x => x.GetByIdAsync(1, false), Times.Once);
    }
    
    [Fact]
    public async Task GetTodoTaskByIdAsync_GivenNonExistingId_ReturnsFailure()
    {
        const int id = 1;
        
        _mockTodoTaskRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync((TodoTask?)null);

        var service = new TodoTaskService(_mockTodoTaskRepository.Object, new Mapper());

        var result = await service.GetTodoTaskByIdAsync(id);

        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal($"Entity with key: {id} was not found", result.Description);
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        _mockTodoTaskRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>(), false), Times.Once);
    }

    [Fact]
    public async Task GetTodoTasksPagedAsync_ValidPagedRequest_ReturnsSuccess()
    {
        const int pageNumber = 1;
        const int pageSize = 1;
        
        var pageddResult = new PagedResult<TodoTask>([
            new TodoTask()
            {
                Id = 1,
                Description = string.Empty,
                DueDate = DateOnly.MaxValue,
                Name = "123",
                Status = Status.InProgress
            }
        ], pageNumber, pageSize, Math.Ceiling((double)1 / pageSize), 1);

        TodoTaskDto[] expectedDtos = [
            new TodoTaskDto(1, "123", string.Empty, DateOnly.MaxValue, Status.InProgress)
        ];
        
        _mockTodoTaskRepository.Setup(x => x.GetPagedAsync(pageNumber, pageSize, false)).ReturnsAsync(pageddResult);

        var service = new TodoTaskService(_mockTodoTaskRepository.Object, new Mapper());
        var result = await service.GetTodoTasksPagedAsync(pageNumber, pageSize);

        Assert.True(result.Success);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Empty(result.Description);
        Assert.Null(result.Location);
        Assert.NotNull(result.Data);
        Assert.Equal(pageNumber, result.Data.CurrentPage);
        Assert.Equal(pageSize, result.Data.ItemsPerPage);
        Assert.Equal(expectedDtos, result.Data.Items);
        Assert.Equal(Math.Ceiling((double)pageNumber / pageSize), result.Data.TotalPages);
        _mockTodoTaskRepository.Verify(x => x.GetPagedAsync(pageNumber, pageSize, false), Times.Once);
    }
    
    [Theory]
    [InlineData(0, 1, false, "Page Number cannot be less than or equal to zero")]
    [InlineData(1, 0, false, "Page Size cannot be less than or equal to zero")]
    [InlineData(0, 0, false, "Page Number cannot be less than or equal to zero")]
    public async Task GetTodoTasksPagedAsync_InvalidPagedRequest_ReturnsCorrectResponse(int pageNumber, int pageSize, bool isSuccess, string expectedErrorMessage)
    {
        var service = new TodoTaskService(_mockTodoTaskRepository.Object, new Mapper());
    
        var result = await service.GetTodoTasksPagedAsync(pageNumber, pageSize);
        
        Assert.False(result.Success);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal(expectedErrorMessage, result.Description);
    }

    [Fact]
    public async Task UpdateTodoTaskAsync_ValidRequest_ReturnsSuccess()
    {
        const int id = 1;
        var request = new UpdateTodoTaskRequest("First updated", "123", DateOnly.MaxValue, Status.Done);

        var todo = new TodoTask()
        {
            Id = 1,
            Description = "",
            DueDate = DateOnly.MaxValue,
            Name = "First",
            Status = Status.InProgress
        };

        var expectedDto = new TodoTaskDto(1, "First updated", "123", DateOnly.MaxValue, Status.Done);
        
        _mockTodoTaskRepository.Setup(x => x.GetByIdAsync(id, true)).ReturnsAsync(todo);

        var service = new TodoTaskService(_mockTodoTaskRepository.Object, new Mapper());

        var result = await service.UpdateTodoTaskAsync(id, request);

        Assert.True(result.Success);
        Assert.Null(result.Location);
        Assert.Empty(result.Description);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Data);
        Assert.Equal(expectedDto.Description, result.Data.Description);
        Assert.Equal(expectedDto.Id, result.Data.Id);
        Assert.Equal(expectedDto.DueDate, result.Data.DueDate);
        Assert.Equal(expectedDto.Status, result.Data.Status);
        Assert.Equal(expectedDto.Name, result.Data.Name);
        _mockTodoTaskRepository.Verify(x => x.GetByIdAsync(id, true), Times.Once);
        _mockTodoTaskRepository.Verify(x => x.UpdateAsync(todo), Times.Once);
    }
    
    [Fact]
    public async Task UpdateTodoTaskAsync_EntityWithGivenIdDoesNotExist_ReturnsSuccess()
    {
        const int id = 1;
        var request = new UpdateTodoTaskRequest("First updated", "123", DateOnly.MaxValue, Status.Done);

        _mockTodoTaskRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), true)).ReturnsAsync((TodoTask?)null);

        var service = new TodoTaskService(_mockTodoTaskRepository.Object, new Mapper());

        var result = await service.UpdateTodoTaskAsync(id, request);

        Assert.False(result.Success);
        Assert.Null(result.Location);
        Assert.Equal($"Entity with key: {id} was not found", result.Description);
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        Assert.Null(result.Data);
        _mockTodoTaskRepository.Verify(x => x.GetByIdAsync(id, true), Times.Once);
    }

    [Theory]
    [InlineData("", 2050, 1, 1, "Name cannot be empty")]
    [InlineData("    ", 2050, 1, 1, "Name cannot be empty")]
    [InlineData(null, 2050, 1, 1, "Name cannot be empty")]
    [InlineData("123", 2000, 1, 1, "Due Date cannot be past")]
    public async Task UpdateTodoTaskAsync_InvalidRequest_ReturnsFailure(string name, int year, int month, int day, string expectedErrorMessage)
    {
        const int id = 1;
        UpdateTodoTaskRequest request = new UpdateTodoTaskRequest(name, "", new DateOnly(year, month, day), Status.Done);

        var service = new TodoTaskService(_mockTodoTaskRepository.Object, new Mapper());

        var result = await service.UpdateTodoTaskAsync(id, request);
        
        Assert.False(result.Success);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal(expectedErrorMessage, result.Description);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task DeleteTodoAsync_GivenExistingEntityId_DeletesEntity()
    {
        const int id = 1;
        var todo = new TodoTask()
        {
            Id = 1,
            Description = "123",
            DueDate = DateOnly.MaxValue,
            Name = "First",
            Status = Status.Todo
        };

        _mockTodoTaskRepository.Setup(x => x.GetByIdAsync(id, false)).ReturnsAsync(todo);
        _mockTodoTaskRepository.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask);

        var service = new TodoTaskService(_mockTodoTaskRepository.Object, new Mapper());
        var result = await service.DeleteTodoTaskAsync(id);
        
        Assert.True(result.Success);
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        Assert.Equal(string.Empty, result.Description);
        Assert.Null(result.Location);
        _mockTodoTaskRepository.Verify(x => x.GetByIdAsync(id, false), Times.Once);
        _mockTodoTaskRepository.Verify(x => x.DeleteAsync(id), Times.Once);
    }
    
    
    [Fact]
    public async Task DeleteTodoAsync_GivenNonExistingEntityId_ReturnsFailure()
    {
        const int id = 1;

        _mockTodoTaskRepository.Setup(x => x.GetByIdAsync(id, false)).ReturnsAsync((TodoTask?)null);
        _mockTodoTaskRepository.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask);

        var service = new TodoTaskService(_mockTodoTaskRepository.Object, new Mapper());
        var result = await service.DeleteTodoTaskAsync(id);
        
        Assert.False(result.Success);
        Assert.False(result.Data);
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        Assert.Equal($"Entity with key: {id} was not found", result.Description);
        Assert.Null(result.Location);
        _mockTodoTaskRepository.Verify(x => x.GetByIdAsync(id, false), Times.Once);
        _mockTodoTaskRepository.Verify(x => x.DeleteAsync(id), Times.Never);
    }
}