using Shared.BaseRequest;

namespace Todo.BussinessLayer.Dtos.Requests;

public record CreateTodoTaskRequest(
    string Name,
    string Description,
    DateOnly DueDate) : IRequest
{
    public ValidationResult Validate()
    {
        List<string> errors = [];

        if (string.IsNullOrWhiteSpace(Name))
        {
            errors.Add("Name cannot be empty");
        }

        if (DueDate < DateOnly.FromDateTime(DateTime.Now))
        {
            errors.Add("Due Date cannot be past");
        }

        return errors.Count == 0
            ? new ValidationResult(true)
            : new ValidationResult(false, errors[0]);
    }
}