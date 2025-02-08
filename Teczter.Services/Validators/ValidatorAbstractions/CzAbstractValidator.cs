using Teczter.Domain;

namespace Teczter.Services.Validators.ValidatorAbstractions;

public abstract class CzAbstractValidator<T>
{
    protected List<Func<T, CzValidationResult>> _validationRules = [];

    public List<CzValidationResult> Validate(T subject)
    {
        var failedValidations = new List<CzValidationResult>();

        foreach (var validation in _validationRules)
        {
            var result = validation(subject);

            if (!result.Success)
            {
                failedValidations.Add(result);
            }
        }

        if (failedValidations.Count > 0)
        {
            return failedValidations;
        }

        return [CzValidationResult.Succeed()];
    }
}