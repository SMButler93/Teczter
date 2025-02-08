using Teczter.Domain.Entities;
using Teczter.Services.Validators.ValidatorAbstractions;

namespace Teczter.Services.Validators;

public class TestStepValidator : CzAbstractValidator<TestStepEntity>
{
    public TestStepValidator()
    {
        _validationRules =
            [

            ];
    }
}
