namespace Teczter.Domain.Exceptions;

public class TeczterGenericException: Exception
{
    public TeczterGenericException(): base("An error has occured.")
    {
    }

    public TeczterGenericException(string message): base(message)
    {
    }

    public TeczterGenericException(string message, Exception innerException): base(message, innerException)
    {
    }
}
