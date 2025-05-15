using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Models;

namespace Todo.DataAccess.Database;

public class TodoDbContext : DbContext
{
    public const string ConnectionStringKey = "Default";
    
    public DbSet<TodoTask> TodoTasks => Set<TodoTask>();
    
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}