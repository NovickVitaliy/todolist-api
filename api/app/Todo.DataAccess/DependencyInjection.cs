using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todo.DataAccess.Database;
using Todo.DataAccess.Repositories.Contracts;
using Todo.DataAccess.Repositories.Implementations;

namespace Todo.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection RegisterDataAccessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TodoDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString(TodoDbContext.ConnectionStringKey)));

        services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
        
        return services;
    }
}