using Microsoft.EntityFrameworkCore;
using Todo.DataAccess.Database;

namespace Todo.API.Extensions;

public static class DatabaseMigrationExtensions
{
    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}