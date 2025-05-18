using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
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

    public static ILoggingBuilder ConfigureOpenTelemetryLogging(this ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("API"));
            options.AddOtlpExporter(loggerOptions =>
            {
                loggerOptions.Endpoint = new Uri("http://localhost:4317");
            });
        });

        return loggingBuilder;
    }

    public static IServiceCollection ConfigureOpenTelemetryTracingAndMetrics(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resourceBuilder => resourceBuilder.AddService(serviceName: "API"))
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddSource("API")
                    .AddOtlpExporter(otlp =>
                    {
                        otlp.Endpoint = new Uri("http://localhost:4317");
                    });
            })
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddMeter("API")
                    .AddOtlpExporter(otlp =>
                    {
                        otlp.Endpoint = new Uri("http://localhost:4317");
                    });
            });

        return services;
    }
}