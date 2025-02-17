using Teczter.Domain;

namespace Teczter.Services.Validation.Validators;

public interface IValidator<T>
{
    public TeczterValidationResult Validate(T subject);
}
