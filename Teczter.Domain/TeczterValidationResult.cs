namespace Teczter.Domain;

public class TeczterValidationResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }

    public static TeczterValidationResult Succeed()
    {
        return new TeczterValidationResult
        {
            Success = true
        };
    }

    public static TeczterValidationResult Fail(string message)
    {
        return new TeczterValidationResult
        {
            Success = false,
            Message = message
        };
    }
}
