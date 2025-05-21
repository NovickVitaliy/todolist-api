using System.Net;
using Moq;
using Shared;
using Shared.ErrorHandling;
using Todo.Application.Todo.Create;
using Todo.Application.Todo.Delete;
using Todo.Application.Todo.GetById;
using Todo.Application.Todo.GetPaged;
using Todo.Application.Todo.Update;
using Todo.BussinessLayer.Dtos;
using Todo.BussinessLayer.Dtos.Requests;
using Todo.BussinessLayer.Services.Contracts;
using Todo.Domain.Models;

namespace TodoList.Tests.Tests.CQRS_HandlersTests;

public class CqrsTests
{
    private readonly Mock<ITodoTaskService> _mockTodoTaskService;
    
    public CqrsTests()
    {
        _mockTodoTaskService = new Mock<ITodoTaskService>();
    }

    [Fact]
    public async Task CreateTodoTask_ValidRequest_ReturnsCorrectResponse()
    {
        var request = new CreateTodoTaskRequest("first", "123", DateOnly.MaxValue);
        var cmd = new CreateTodoTaskCommand(request);
        var expectedResponse = Result<TodoTaskDto>.Created("/api/todos/1", new TodoTaskDto(1, "first", "123", DateOnly.MaxValue, Status.Todo));
        _mockTodoTaskService.Setup(x => x.CreateTodoTaskAsync(request)).ReturnsAsync(expectedResponse);
        
        var handler = new CreateTodoTaskCommandHandler(_mockTodoTaskService.Object);

        var result = await handler.Handle(cmd, CancellationToken.None);
        
        Assert.True(result.Success);
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        Assert.Equal(expectedResponse.Data.Id, result.Data.Id);
        Assert.Equal(expectedResponse.Data.Name, result.Data.Name);
        Assert.Equal(expectedResponse.Data.Description, result.Data.Description);
        Assert.Equal(expectedResponse.Data.DueDate, result.Data.DueDate);
        Assert.Equal(expectedResponse.Data.Status, result.Data.Status);
        _mockTodoTaskService.Verify(x => x.CreateTodoTaskAsync(request), Times.Once);
    }
    
    [Fact]
    public async Task DeleteTodoTask_ValidRequest_ReturnsCorrectResponse()
    {
        const int id = 1;
        var cmd = new DeleteTodoTaskCommand(id);
        var expectedResponse = Result<bool>.NoContent();
        _mockTodoTaskService.Setup(x => x.DeleteTodoTaskAsync(id)).ReturnsAsync(expectedResponse);
        
        var handler = new DeleteTodoTaskCommandHandler(_mockTodoTaskService.Object);

        var result = await handler.Handle(cmd, CancellationToken.None);
        
        Assert.True(result.Success);
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        _mockTodoTaskService.Verify(x => x.DeleteTodoTaskAsync(id), Times.Once);
    }
    
    [Fact]
    public async Task GetById_ValidRequest_ReturnsCorrectResponse()
    {
        const int id = 1;
        var cmd = new GetTodoTaskByIdQuery(id);
        var expectedDto = new TodoTaskDto(1, "first", "123", DateOnly.MaxValue, Status.Done);
        var expectedResponse = Result<TodoTaskDto>.Ok(expectedDto);
        _mockTodoTaskService.Setup(x => x.GetTodoTaskByIdAsync(id)).ReturnsAsync(expectedResponse);
        
        var handler = new GetTodoTaskByIdQueryHandler(_mockTodoTaskService.Object);

        var result = await handler.Handle(cmd, CancellationToken.None);
        
        Assert.True(result.Success);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(expectedResponse.Data.Id, result.Data.Id);
        Assert.Equal(expectedResponse.Data.Name, result.Data.Name);
        Assert.Equal(expectedResponse.Data.Description, result.Data.Description);
        Assert.Equal(expectedResponse.Data.DueDate, result.Data.DueDate);
        Assert.Equal(expectedResponse.Data.Status, result.Data.Status);
        _mockTodoTaskService.Verify(x => x.GetTodoTaskByIdAsync(id), Times.Once);
    }
    
    [Fact]
    public async Task GetById_GivenNonExistingEntityId_ReturnsCorrectResponse()
    {
        const int id = 1;
        var cmd = new GetTodoTaskByIdQuery(id);
        var expectedResponse = Result<TodoTaskDto>.NotFound(id);
        _mockTodoTaskService.Setup(x => x.GetTodoTaskByIdAsync(id)).ReturnsAsync(expectedResponse);
        
        var handler = new GetTodoTaskByIdQueryHandler(_mockTodoTaskService.Object);

        var result = await handler.Handle(cmd, CancellationToken.None);
        
        Assert.False(result.Success);
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        Assert.Null(result.Data);
        Assert.Equal($"Entity with key: {id} was not found", result.Description);
        Assert.Null(result.Location);
        _mockTodoTaskService.Verify(x => x.GetTodoTaskByIdAsync(id), Times.Once);
    }
    
    [Fact]
    public async Task GetPaged_GivenValidRequest_ReturnsCorrectResponse()
    {
        const int id = 1;
        const int pageNumber = 1;
        const int pageSize = 1;
        TodoTaskDto[] expectedDtos = [
            new TodoTaskDto(1, "first", "123", DateOnly.MaxValue, Status.Done)
        ];
        var cmd = new GetPagedTodoTaskQuery(pageNumber, pageSize);
        var expectedResponse = Result<PagedResult<TodoTaskDto>>.Ok(
                new PagedResult<TodoTaskDto>(expectedDtos, pageNumber, pageSize, (double)pageNumber / pageSize, 1));
        _mockTodoTaskService.Setup(x => x.GetTodoTasksPagedAsync(pageNumber, pageSize)).ReturnsAsync(expectedResponse);
        
        var handler = new GetPagedTodoTaskQueryHandler(_mockTodoTaskService.Object);

        var result = await handler.Handle(cmd, CancellationToken.None);
        
        Assert.True(result.Success);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Data);
        Assert.Equal(string.Empty, result.Description);
        Assert.Null(result.Location);
        Assert.Equal(expectedDtos, result.Data.Items);
        Assert.Equal(expectedResponse.Data.ItemsPerPage, result.Data.ItemsPerPage);
        Assert.Equal(expectedResponse.Data.CurrentPage, result.Data.CurrentPage);
        Assert.Equal(expectedResponse.Data.TotalPages, result.Data.TotalPages);
        _mockTodoTaskService.Verify(x => x.GetTodoTasksPagedAsync(pageNumber, pageSize), Times.Once);
    }

    [Fact]
    public async Task UpdateTodoTaskCommand_ValidRequest_ReturnsSuccess()
    {
        const int id = 1;
        var request = new UpdateTodoTaskRequest("First updated", "123", DateOnly.MaxValue, Status.Done);
        var cmd = new UpdateTodoTaskCommand(id, request);

        var expectedDto = new TodoTaskDto(1, "First updated", "123", DateOnly.MaxValue, Status.Done);
        var expectedResponse = Result<TodoTaskDto>.Ok(expectedDto);

        _mockTodoTaskService.Setup(x => x.UpdateTodoTaskAsync(1, request)).ReturnsAsync(expectedResponse);

        var handler = new UpdateTodoTaskCommandHandler(_mockTodoTaskService.Object);

        var response = await handler.Handle(cmd, CancellationToken.None);

        Assert.True(response.Success);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Null(response.Location);
        Assert.Empty(response.Description);
        Assert.Equal(expectedDto.Id, response.Data.Id);
        Assert.Equal(expectedDto.Description, response.Data.Description);
        Assert.Equal(expectedDto.DueDate, response.Data.DueDate);
        Assert.Equal(expectedDto.Status, response.Data.Status);
        Assert.Equal(expectedDto.Name, response.Data.Name);
        _mockTodoTaskService.Verify(x => x.UpdateTodoTaskAsync(1, request), Times.Once);
    }
}