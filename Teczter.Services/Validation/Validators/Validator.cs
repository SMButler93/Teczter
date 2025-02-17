using Teczter.Domain;
using Teczter.Services.Builders;
using Teczter.Services.Validation.ValidationRules.ValidationRulesProvider;

namespace Teczter.Services.Validation.Validators
{
    public class Validator<T>(AbstractValidationRulesProvider<T> ruleProvider) : IValidator<T>
    {
        private readonly List<Func<T, TeczterValidationResult>> _rules = ruleProvider.GetRules();

        public TeczterValidationResult Validate(T subject)
        {
            var results = _rules.Select(validation => validation(subject));

            var failures = results.Where(x => !x.Success).ToArray();

            if (failures.Length > 0)
            {
                var errorMessage = ErrorMessageFormatter.CreateValidationErrorMessage(failures);
                return TeczterValidationResult.Fail(errorMessage);
            }

            return TeczterValidationResult.Succeed();
        }
    }
}
