namespace Teczter.Domain.Exceptions;

public class CzValidationException : Exception
{
    public CzValidationException(): base("A validation error has occured.")
    {
    }

    public CzValidationException(string message) : base(message)
    {
    }

    public CzValidationException(string message, Exception innerException): base(message, innerException)
    {
    }
}
