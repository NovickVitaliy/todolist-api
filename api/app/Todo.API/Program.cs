using Todo.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplicationLayer(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();