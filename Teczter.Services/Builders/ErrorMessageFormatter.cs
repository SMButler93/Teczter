using System.Text;
using Teczter.Domain;

namespace Teczter.Services.Builders;

public static class ErrorMessageFormatter
{
    public static string CreateValidationErrorMessage(List<CzValidationResult> errors)
    {
        var sb = new StringBuilder();
        var errorCount = 1;

        sb.AppendLine("Validation failed for the following reasons:");

        foreach (var error in errors)
        {
            sb.AppendLine($"\n\t{errorCount}. {error.Message}");
            errorCount++;
        }

        return sb.ToString();
    }
}