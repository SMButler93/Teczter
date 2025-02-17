using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Services.Validation.ValidationRules.ValidationRulesProvider;

namespace Teczter.Services.Validation.ValidationRules;

public class TestValidationRuleProvider : AbstractValidationRulesProvider<TestEntity>
{
    public override List<Func<TestEntity, CzValidationResult>> GetRules()
    {
        return GetRules(typeof(TestValidationRules));
    }
}

public class TestStepValidationRuleProvider : AbstractValidationRulesProvider<TestStepEntity>
{
    public override List<Func<TestStepEntity, CzValidationResult>> GetRules()
    {
        return GetRules(typeof(TestStepValidationRules));
    }
}