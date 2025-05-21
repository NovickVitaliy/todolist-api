using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Shared;
using Todo.API.Extensions;
using Todo.Application;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOpenTelemetryTracingAndMetrics();

builder.Host.ConfigureSerilogLogging();

builder.Logging.ConfigureOpenTelemetryLogging();

builder.Services.RegisterApplicationLayer(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

var app = builder.Build();

app.UseCors();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    await app.MigrateDatabaseAsync();
}

app.MapControllers();

app.Run();