﻿namespace Teczter.Domain.Exceptions;

public class TeczterValidationException : Exception
{
    public TeczterValidationException() : base("A validation error has occurred.")
    {
    }

    public TeczterValidationException(string message) : base(message)
    {
    }

    public TeczterValidationException(string message, Exception innerException): base(message, innerException)
    {
    }
}
