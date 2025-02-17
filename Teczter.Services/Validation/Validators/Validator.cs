using Teczter.Domain;
using Teczter.Services.Builders;
using Teczter.Services.Validation.ValidationRules.ValidationRulesProvider;

namespace Teczter.Services.Validation.Validators
{
    public class Validator<T>(AbstractValidationRulesProvider<T> ruleProvider) : IValidator<T>
    {
        private readonly List<Func<T, CzValidationResult>> _rules = ruleProvider.GetRules();

        public CzValidationResult Validate(T subject)
        {
            var results = _rules.Select(validation => validation(subject));

            var failures = results.Where(x => !x.Success).ToArray();

            if (failures.Length > 0)
            {
                var errorMessage = ErrorMessageFormatter.CreateValidationErrorMessage(failures);
                return CzValidationResult.Fail(errorMessage);
            }

            return CzValidationResult.Succeed();
        }
    }
}
