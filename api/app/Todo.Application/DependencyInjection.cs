using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.CQRS.PipelineBehaviours;
using Todo.BussinessLayer;

namespace Todo.Application;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterBussinessLayer(configuration);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(LoggingPipelineBehaviour<,>));
        });

        return services;
    }
}