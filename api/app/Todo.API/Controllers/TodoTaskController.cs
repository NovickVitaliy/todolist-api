using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Todo.Create;
using Todo.Application.Todo.Delete;
using Todo.Application.Todo.GetById;
using Todo.Application.Todo.GetPaged;
using Todo.Application.Todo.Update;
using Todo.BussinessLayer.Dtos.Requests;

namespace Todo.API.Controllers;

[Route("api/todos")]
[ApiController]
public class TodoTaskController : ControllerBase
{
    private readonly ISender _sender;
    
    public TodoTaskController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateTodoTaskRequest request)
    {
        return (await _sender.Send(new CreateTodoTaskCommand(request))).ToApiResponse();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        return (await _sender.Send(new GetTodoTaskByIdQuery(id))).ToApiResponse();
    }

    [HttpGet]
    public async Task<IActionResult> GetPagedAsync(int pageNumber = 1, int pageSize = 10)
    {
        return (await _sender.Send(new GetPagedTodoTaskQuery(pageNumber, pageSize))).ToApiResponse();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, UpdateTodoTaskRequest request)
    {
        return (await _sender.Send(new UpdateTodoTaskCommand(id, request))).ToApiResponse();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _sender.Send(new DeleteTodoTaskCommand(id));
        return NoContent();
    }
}