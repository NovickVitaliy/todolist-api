using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Todo.DataAccess.Database;
using Todo.DataAccess.Repositories.Implementations;
using Todo.Domain.Models;
using TodoList.Tests.Mocks;

namespace TodoList.Tests.Tests.Repository;

public class TodoTaskRepositoryTests
{
    private readonly Mock<TodoDbContext> _mockDbContext;

    public TodoTaskRepositoryTests()
    {
        _mockDbContext = new Mock<TodoDbContext>(new DbContextOptions<TodoDbContext>());
    }

    [Fact]
    public async Task CreateAsync_GivenNewTodoTask_AssignId()
    {
        var mockDbSet = new Mock<DbSet<TodoTask>>();
        _mockDbContext.Setup(x => x.Set<TodoTask>()).Returns(mockDbSet.Object);
        _mockDbContext.Setup(x => x.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

        var repository = new TodoTaskRepository(_mockDbContext.Object);
        var todo = new TodoTask()
        {
            Id = 1,
            Description = "123",
            DueDate = DateOnly.MaxValue,
            Name = "123",
            Status = Status.Todo
        };

        var createdTodo = await repository.CreateAsync(todo);

        Assert.NotNull(createdTodo);
        Assert.Equal(todo, createdTodo);
        _mockDbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        mockDbSet.Verify(x => x.Add(It.Is<TodoTask>(t => t == todo)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_GivenValidEntity_UpdateTodoTask()
    {
        var mockDbSet = new Mock<DbSet<TodoTask>>();
        _mockDbContext.Setup(x => x.Set<TodoTask>()).Returns(mockDbSet.Object);
        _mockDbContext.Setup(x => x.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

        var repository = new TodoTaskRepository(_mockDbContext.Object);
        var todo = new TodoTask()
        {
            Id = 1,
            Description = "123",
            DueDate = DateOnly.MaxValue,
            Name = "123",
            Status = Status.InProgress
        };

        var result = await repository.UpdateAsync(todo);

        Assert.Equal(todo, result);
        Assert.NotNull(result);
        mockDbSet.Verify(x => x.Update(It.Is<TodoTask>(t => t == todo)), Times.Once);
        _mockDbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_GivenExistingTodoId_ReturnsTodoTaskTracking()
    {
        List<TodoTask> list = GetTestData();
        var (mockDbContext, mockDbSet) = DbContextMock.GetMockDbContext<TodoTask, TodoDbContext>(list, context => context.Set<TodoTask>());

        var repository = new TodoTaskRepository(mockDbContext.Object);
        const int taskId = 1;
        var todo = await repository.GetByIdAsync(taskId, false);
        
        Assert.NotNull(todo);
        Assert.Equal(1, todo.Id);
    }
    
    [Fact]
    public async Task GetByIdAsync_GivenExistingTodoId_ReturnsTodoTaskNonTracking()
    {
        List<TodoTask> list = GetTestData();
        var (mockDbContext, mockDbSet) = DbContextMock.GetMockDbContext<TodoTask, TodoDbContext>(list, context => context.Set<TodoTask>());

        var repository = new TodoTaskRepository(mockDbContext.Object);
        const int taskId = 1;
        var todo = await repository.GetByIdAsync(taskId, false);
        
        Assert.NotNull(todo);
        Assert.Equal(1, todo.Id);
    }
    
    [Fact]
    public async Task GetByIdAsync_GivenNonExistingTodoId_ReturnsNull()
    {
        List<TodoTask> list = GetTestData();
        var (mockDbContext, mockDbSet) = DbContextMock.GetMockDbContext<TodoTask, TodoDbContext>(list, context => context.Set<TodoTask>());

        var repository = new TodoTaskRepository(mockDbContext.Object);
        const int taskId = 3;
        var todo = await repository.GetByIdAsync(taskId, false);

        Assert.Null(todo);
    }

    [Fact]
    public async Task GetPagedAsync_GivenValidPage_ReturnsPagedResult()
    {
        var todoTasks = GetTestData();
        var (mockDbContext, mockDbSet) = DbContextMock.GetMockDbContext<TodoTask, TodoDbContext>(todoTasks, context => context.Set<TodoTask>());
        var repository = new TodoTaskRepository(mockDbContext.Object);

        var result = await repository.GetPagedAsync(1, 2, false);
        
        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(2, result.ItemsPerPage);
        Assert.Equal(1, result.TotalPages);
        Assert.Equal("First", result.Items[0].Name);
        Assert.Equal(1, result.Items[0].Id);
        Assert.Equal("Second", result.Items[1].Name);
        Assert.Equal(2, result.Items[1].Id);
    }

    [Fact]
    public async Task DeleteAsync_GivenExistingId_DeletesRecord()
    {
        var todoTasks = GetTestData();
        var (mockDbContext, mockDbSet) = DbContextMock.GetMockDbContext<TodoTask, TodoDbContext>(todoTasks, context => context.Set<TodoTask>());
        var repository = new TodoTaskRepository(mockDbContext.Object);

        await repository.DeleteAsync(1);
        
        mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        mockDbContext.Verify(x => x.Remove(It.Is<TodoTask>(t => t.Id == 1)), Times.Once);
    }
    
    private static List<TodoTask> GetTestData()
    {
        return 
        [
            new TodoTask
            {
                Id = 1,
                Name = "First",
                DueDate = DateOnly.MaxValue,
                Status = Status.Todo,
                Description = "123"
            },
            new TodoTask
            {
                Id = 2,
                Name = "Second",
                DueDate = DateOnly.MaxValue,
                Status = Status.Todo,
                Description = "123"
            },
        ];
    }
}