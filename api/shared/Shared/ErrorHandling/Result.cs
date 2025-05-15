using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Shared.ErrorHandling;

public class Result<T>
{
    public string Description { get; }
    public HttpStatusCode StatusCode { get; }
    public T Data { get; }
    public string? Location { get; }
    public bool Success { get; }

    private Result(string description, HttpStatusCode statusCode, T data, bool success, string? location = null)
    {
        Description = description;
        StatusCode = statusCode;
        Data = data;
        Success = success;
        Location = location;
    }

    public static Result<T> Ok(T data)
    {
        return new Result<T>(string.Empty, HttpStatusCode.OK, data, true);
    }

    public static Result<T> NoContent()
    {
        return new Result<T>(string.Empty, HttpStatusCode.NoContent, default!, true);
    }

    public static Result<T> Created(string location, T value)
    {
        return new Result<T>(string.Empty, HttpStatusCode.Created, value, true, location);
    }

    public static Result<T> Accepted(T data, string location)
    {
        return new Result<T>(string.Empty, HttpStatusCode.Accepted, data, true);
    }

    public static Result<T> BadRequest(string reason)
    {
        return new Result<T>(reason, HttpStatusCode.BadRequest, default!, false);
    }

    public static Result<T> Unauthorized(string reason = "Unauthorized")
    {
        return new Result<T>(reason, HttpStatusCode.Unauthorized, default!, false);
    }


    public static Result<T> Forbidden(string reason = "Forbidden")
    {
        return new Result<T>(reason, HttpStatusCode.Forbidden, default!, false);
    }

    public static Result<T> NotFound(object key)
    {
        return new Result<T>($"Entity with key: {key} was not found", HttpStatusCode.NotFound, default!, false);
    }

    public static Result<T> Conflict(string reason)
    {
        return new Result<T>(reason, HttpStatusCode.Conflict, default!, false);
    }

    public static Result<T> InternalServerError(string reason = "Internal Server Error")
    {
        return new Result<T>(reason, HttpStatusCode.InternalServerError, default!, false);
    }

    public IActionResult ToApiResponse()
    {
        ProblemDetails? problemDetails = null;

        if ((int)StatusCode >= 400)
        {
            problemDetails = new ProblemDetails()
            {
                Status = (int)StatusCode,
                Title = "An error occurred",
                Detail = Description,
                Type = StatusCode.ToString()
            };
        }

        return StatusCode switch
        {
            HttpStatusCode.OK => new OkObjectResult(Data),
            HttpStatusCode.NoContent => new NoContentResult(),
            HttpStatusCode.Created => new CreatedResult(Location, Data),
            HttpStatusCode.Accepted => new AcceptedResult(Location, Data),
            HttpStatusCode.BadRequest => new BadRequestObjectResult(problemDetails),
            HttpStatusCode.Unauthorized => new ObjectResult(problemDetails) {StatusCode = (int?)HttpStatusCode.Unauthorized},
            HttpStatusCode.Forbidden => new ObjectResult(problemDetails) { StatusCode = (int)HttpStatusCode.Forbidden },
            HttpStatusCode.NotFound => new NotFoundObjectResult(problemDetails),
            HttpStatusCode.Conflict => new ConflictObjectResult(problemDetails),
            _ => new ObjectResult(problemDetails) { StatusCode = (int)HttpStatusCode.InternalServerError }
        };
    }
}