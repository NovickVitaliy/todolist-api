namespace Shared.BaseRequest;

public interface IRequest
{
    ValidationResult Validate();
}