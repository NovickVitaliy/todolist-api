namespace Shared.BaseRequest;

public record ValidationResult(bool IsValid, string ErrorMessage = "");