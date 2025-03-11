namespace Teczter.Domain;

public record TeczterValidationResult<T>
{
    public bool IsValid { get; init; }
    public string[]? ErrorMessages { get; init; }
    public T? Value { get; init; }

    public static TeczterValidationResult<T> Succeed(T value)
    {
        return new TeczterValidationResult<T>
        {
            IsValid = true,
            ErrorMessages = [],
            Value = value
        };
    }

    public static TeczterValidationResult<T> Fail(string[] errorMessages)
    {
        return new TeczterValidationResult<T>
        {
            IsValid = false,
            ErrorMessages = errorMessages,
            Value = default
        };
    }
}