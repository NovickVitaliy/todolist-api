using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todo.BussinessLayer.Services.Contracts;
using Todo.BussinessLayer.Services.Implementations;
using Todo.DataAccess;

namespace Todo.BussinessLayer;

public static class DependencyInjection
{
    public static IServiceCollection RegisterBussinessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterDataAccessLayer(configuration);
        services.AddMapster();

        services.AddScoped<ITodoTaskService, TodoTaskService>();
        
        return services;
    }
}