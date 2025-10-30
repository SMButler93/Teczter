using System.Text;

namespace Teczter.Services.ComposersAndBuilders;

public static class ErrorMessageResponseBuilder
{
    public static string BuildErrorMessage(string[] errorMessages)
    {
        var sb = new StringBuilder();

        var messageStart = "An error occured because of the following reasons:";

        sb.AppendLine(messageStart);

        for (int i = 0; i < errorMessages.Length; i++)
        {
            sb.AppendLine($"{i + 1}. {errorMessages[i]}");
        }

        return sb.ToString();
    }
}
