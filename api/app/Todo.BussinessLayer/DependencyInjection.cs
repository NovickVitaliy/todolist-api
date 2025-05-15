using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todo.DataAccess;

namespace Todo.BussinessLayer;

public static class DependencyInjection
{
    public static IServiceCollection RegisterBussinessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterDataAccessLayer(configuration);
        
        return services;
    }
}