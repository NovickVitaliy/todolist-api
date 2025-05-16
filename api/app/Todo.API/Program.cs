using Microsoft.AspNetCore.Mvc;
using Todo.API.Extensions;
using Todo.Application;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplicationLayer(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.MigrateDatabaseAsync();
}

app.MapControllers();

app.Run();