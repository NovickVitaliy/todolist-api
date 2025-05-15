using Todo.API.Extensions;
using Todo.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplicationLayer(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.MigrateDatabaseAsync();
}

app.MapGet("/", () => "Hello World!");

app.Run();