namespace Teczter.Domain.Exceptions;

public class CzGenericException: Exception
{
    public CzGenericException(): base("An error has occured.")
    {
    }

    public CzGenericException(string message): base(message)
    {
    }

    public CzGenericException(string message, Exception innerException): base(message, innerException)
    {
    }
}
