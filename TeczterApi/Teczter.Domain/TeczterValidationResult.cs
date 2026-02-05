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

    public static TeczterValidationResult<T> Fail(string errorMessage)
    {
        return new TeczterValidationResult<T>
        {
            IsValid = false,
            ErrorMessages = [errorMessage],
            Value = default
        };
    }
}

public record TeczterValidationResult
{
    public bool IsValid { get; init; }
    public string[]? ErrorMessages { get; init; }

    public static TeczterValidationResult Succeed()
    {
        return new TeczterValidationResult
        {
            IsValid = true,
            ErrorMessages = []
        };
    }

    public static TeczterValidationResult Fail(string[] errorMessages)
    {
        return new TeczterValidationResult
        {
            IsValid = false,
            ErrorMessages = errorMessages
        };
    }

    public static TeczterValidationResult Fail(string errorMessage)
    {
        return new TeczterValidationResult
        {
            IsValid = false,
            ErrorMessages = [errorMessage]
        };
    }
}