namespace Teczter.Domain;

public class CzValidationResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }

    public static CzValidationResult Succeed()
    {
        return new CzValidationResult
        {
            Success = true
        };
    }

    public static CzValidationResult Fail(string message)
    {
        return new CzValidationResult
        {
            Success = false,
            Message = message
        };
    }
}
