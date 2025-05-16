using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Shared;

public static class DependencyInjection
{
    public static ConfigureHostBuilder ConfigureSerilogLogging(this ConfigureHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, configuration) => 
            configuration.ReadFrom.Configuration(context.Configuration));

        return hostBuilder;
    }
}