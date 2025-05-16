using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared;
using Todo.API.Extensions;
using Todo.Application;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureSerilogLogging();

builder.Services.RegisterApplicationLayer(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    await app.MigrateDatabaseAsync();
}

app.MapControllers();

app.Run();