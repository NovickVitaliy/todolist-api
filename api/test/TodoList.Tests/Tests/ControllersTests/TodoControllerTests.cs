using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared;
using Shared.ErrorHandling;
using Todo.API.Controllers;
using Todo.Application.Todo.Create;
using Todo.Application.Todo.Delete;
using Todo.Application.Todo.GetById;
using Todo.Application.Todo.GetPaged;
using Todo.Application.Todo.Update;
using Todo.BussinessLayer.Dtos;
using Todo.BussinessLayer.Dtos.Requests;
using Todo.Domain.Models;

namespace TodoList.Tests.Tests.ControllersTests;

public class TodoControllerTests
{
    private readonly TodoTaskController _todoTaskController;
    private readonly Mock<ISender> _mockSender;

    public TodoControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _todoTaskController = new TodoTaskController(_mockSender.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsCreated()
    {
        var request = new CreateTodoTaskRequest("First", "123", DateOnly.MaxValue);
        var dto = new TodoTaskDto(1, "First", "123", DateOnly.MaxValue, Status.Todo);
        var commandResult = Result<TodoTaskDto>.Created("/api/todos/1", dto);

        _mockSender.Setup(x => x.Send(It.Is<CreateTodoTaskCommand>(cmd => cmd.Request == request), CancellationToken.None))
            .ReturnsAsync(commandResult);

        var result = await _todoTaskController.CreateAsync(request);
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal("/api/todos/1", createdResult.Location);
        var todoTaskDto = Assert.IsType<TodoTaskDto>(createdResult.Value);
        Assert.Equal(dto.Id, todoTaskDto.Id);
    }

    [Theory]
    [InlineData("", 2050, 1, 1, "Name cannot be empty")]
    [InlineData(null, 2050, 1, 1, "Name cannot be empty")]
    [InlineData("   ", 2050, 1, 1, "Name cannot be empty")]
    [InlineData("123", 2000, 1, 1, "Due Date cannot be past")]
    public async Task CreateAsync_InvalidRequest_ReturnsBadRequest(string name, int year, int month, int day, string expectedErrorMessage)
    {
        var request = new CreateTodoTaskRequest(name, string.Empty, new DateOnly(year, month, day));
        var commandResult = Result<TodoTaskDto>.BadRequest(expectedErrorMessage);

        _mockSender.Setup(x => x.Send(It.Is<CreateTodoTaskCommand>(cmd => cmd.Request == request), CancellationToken.None))
            .ReturnsAsync(commandResult);

        var result = await _todoTaskController.CreateAsync(request);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var details = badRequestResult.Value as ProblemDetails;
        Assert.Equal((int?)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        Assert.Equal(expectedErrorMessage, details!.Detail);
    }

    [Fact]
    public async Task GetByIdAsync_GivenIdOfExistingEntity_ReturnsOk()
    {
        const int id = 1;
        var dto = new TodoTaskDto(1, "First", string.Empty, DateOnly.MaxValue, Status.Todo);
        var expectedResult = Result<TodoTaskDto>.Ok(dto);

        _mockSender.Setup(x => x.Send(It.Is<GetTodoTaskByIdQuery>(q => q.Id == id),
                CancellationToken.None))
            .ReturnsAsync(expectedResult);

        var result = await _todoTaskController.GetByIdAsync(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var todoTaskDto = Assert.IsType<TodoTaskDto>(okResult.Value);

        Assert.Equal((int?)HttpStatusCode.OK, okResult.StatusCode);
        Assert.Equal(dto.Id, todoTaskDto.Id);
        Assert.Equal(dto.Description, todoTaskDto.Description);
        Assert.Equal(dto.DueDate, todoTaskDto.DueDate);
        Assert.Equal(dto.Name, todoTaskDto.Name);
        Assert.Equal(dto.Status, todoTaskDto.Status);
    }


    [Fact]
    public async Task GetByIdAsync_GivenIdOfNonExistingEntity_ReturnsNotFound()
    {
        const int id = 1;
        var expectedResult = Result<TodoTaskDto>.NotFound(id);

        _mockSender.Setup(x => x.Send(It.Is<GetTodoTaskByIdQuery>(q => q.Id == id),
                CancellationToken.None))
            .ReturnsAsync(expectedResult);

        var result = await _todoTaskController.GetByIdAsync(id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal((int?)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        var problemDetails = notFoundResult.Value as ProblemDetails;
        Assert.Equal("Entity with key: 1 was not found", problemDetails!.Detail);
    }

    [Fact]
    public async Task GetPagedAsync_GivenValidPaginationRequest_ReturnsOk()
    {
        const int pageNumber = 1;
        const int pageSize = 1;
        TodoTaskDto[] dtos =
        [
            new TodoTaskDto(1, "First", string.Empty, DateOnly.MaxValue, Status.Todo)
        ];

        var expectedResult = Result<PagedResult<TodoTaskDto>>.Ok(new PagedResult<TodoTaskDto>(dtos, pageNumber, pageSize, 1, 1));
        _mockSender.Setup(x => x.Send(It.Is<GetPagedTodoTaskQuery>(q => q.PageNumber == pageNumber && q.PageSize == pageSize), CancellationToken.None))
            .ReturnsAsync(expectedResult);


        var result = await _todoTaskController.GetPagedAsync(pageNumber, pageSize);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal((int?)HttpStatusCode.OK, okResult.StatusCode);
        var pagedResult = Assert.IsType<PagedResult<TodoTaskDto>>(okResult.Value);
        Assert.Equal(dtos, pagedResult.Items);
        Assert.Equal(pageNumber, pagedResult.CurrentPage);
        Assert.Equal(pageSize, pagedResult.ItemsPerPage);
        Assert.Equal(1, pagedResult.TotalPages);
    }
    
    [Theory]
    [InlineData(0, 1, "Page Number cannot be less than or equal to zero")]
    [InlineData(1, 0, "Page Size cannot be less than or equal to zero")]
    public async Task GetPagedAsync_GivenInvalidPaginationRequest_ReturnsOk(int pageNumber, int pageSize, string expectedErrorMessage)
    {

        var expectedResult = Result<PagedResult<TodoTaskDto>>.BadRequest(expectedErrorMessage);
        _mockSender.Setup(x => x.Send(It.Is<GetPagedTodoTaskQuery>(q => q.PageNumber == pageNumber && q.PageSize == pageSize), CancellationToken.None))
            .ReturnsAsync(expectedResult);


        var result = await _todoTaskController.GetPagedAsync(pageNumber, pageSize);

        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal((int?)HttpStatusCode.BadRequest, badRequestObjectResult.StatusCode);
        var problemDetails = badRequestObjectResult.Value as ProblemDetails;
        Assert.Equal(expectedErrorMessage, problemDetails!.Detail);
    }

    [Fact]
    public async Task UpdateAsync_GivenValidRequest_ReturnsOk()
    {
        const int id = 1;
        var request = new UpdateTodoTaskRequest("First updated", string.Empty, DateOnly.MaxValue, Status.InProgress);
        var dto = new TodoTaskDto(1, "First updated", string.Empty, DateOnly.MaxValue, Status.InProgress);
        var expectedResponse = Result<TodoTaskDto>.Ok(dto);

        _mockSender.Setup(x => x.Send(It.Is<UpdateTodoTaskCommand>(cmd => cmd.Request == request), CancellationToken.None))
            .ReturnsAsync(expectedResponse);

        var result = await _todoTaskController.UpdateAsync(id, request);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var todoTaskDto = Assert.IsType<TodoTaskDto>(okObjectResult.Value);
        Assert.Equal((int?)HttpStatusCode.OK, okObjectResult.StatusCode);
        Assert.Equal(dto.Id, todoTaskDto.Id);
        Assert.Equal(dto.Description, todoTaskDto.Description);
        Assert.Equal(dto.DueDate, todoTaskDto.DueDate);
        Assert.Equal(dto.Name, todoTaskDto.Name);
        Assert.Equal(dto.Status, todoTaskDto.Status);
    }

    [Theory]
    [InlineData("", 2050, 1, 1, "Name cannot be empty")]
    [InlineData(null, 2050, 1, 1, "Name cannot be empty")]
    [InlineData("   ", 2050, 1, 1, "Name cannot be empty")]
    [InlineData("123", 2000, 1, 1, "Due Date cannot be past")]
    public async Task UpdateAsync_GivenInvalidRequest_ReturnsBadRequest(string name, int year, int month, int day, string expectedErrorMessage)
    {
        const int id = 1;
        var request = new UpdateTodoTaskRequest(name, string.Empty, new DateOnly(year, month, day), Status.InProgress);
        var expectedResponse = Result<TodoTaskDto>.BadRequest(expectedErrorMessage);

        _mockSender.Setup(x => x.Send(It.Is<UpdateTodoTaskCommand>(cmd => cmd.Request == request), CancellationToken.None))
            .ReturnsAsync(expectedResponse);

        var result = await _todoTaskController.UpdateAsync(id, request);

        var okObjectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal((int?)HttpStatusCode.BadRequest, okObjectResult.StatusCode);
        var problemDetails = okObjectResult.Value as ProblemDetails;
        Assert.Equal(expectedErrorMessage, problemDetails!.Detail);
        Assert.Equal((int?)HttpStatusCode.BadRequest, problemDetails.Status);
    }

    [Fact]
    public async Task DeleteAsync_GivenExistingTodoTaskId_ReturnsNoContent()
    {
        const int id = 1;
        var expectedResponse = Result<bool>.NoContent();
        _mockSender.Setup(x => x.Send(It.Is<DeleteTodoTaskCommand>(x => x.Id == id), CancellationToken.None))
            .ReturnsAsync(expectedResponse);

        var result = await _todoTaskController.DeleteAsync(id);

        var noContentResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal((int?)HttpStatusCode.NoContent, noContentResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteAsync_GivenNonExistingTodoTaskId_ReturnsNotFound()
    {
        const int id = 1;
        var expectedResponse = Result<bool>.NotFound(id);
        _mockSender.Setup(x => x.Send(It.Is<DeleteTodoTaskCommand>(x => x.Id == id), CancellationToken.None))
            .ReturnsAsync(expectedResponse);

        var result = await _todoTaskController.DeleteAsync(id);

        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal((int?)HttpStatusCode.NotFound, notFoundObjectResult.StatusCode);
        var problemDetails = notFoundObjectResult.Value as ProblemDetails;
        Assert.Equal(expectedResponse.Description, problemDetails!.Detail);
        Assert.Equal((int?)HttpStatusCode.NotFound, problemDetails.Status);
    }
}