using System.Text;
using Teczter.Domain;

namespace Teczter.Services.Builders;

public static class ErrorMessageBuilder
{
    public static string CreateValidationErrorMessage(IEnumerable<string> errors)
    {
        var sb = new StringBuilder();
        var errorCount = 0;

        sb.AppendLine("Validation failed for the following reasons:");

        foreach (var error in errors)
        {
            sb.AppendLine($"\n\t{++errorCount}. {error}");
        }

        return sb.ToString();
    }
}