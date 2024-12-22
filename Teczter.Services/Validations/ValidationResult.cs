namespace Teczter.Services.Validations;

public record ValidationResult(bool IsValid, string? ErrorMessage)
{
}
