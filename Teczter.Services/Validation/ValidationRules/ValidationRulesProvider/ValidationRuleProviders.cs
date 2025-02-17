using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Services.Validation.ValidationRules.ValidationRulesProvider;

namespace Teczter.Services.Validation.ValidationRules;

public class TestValidationRuleProvider : AbstractValidationRulesProvider<TestEntity>
{
    public override List<Func<TestEntity, TeczterValidationResult>> GetRules()
    {
        return GetRules(typeof(TestValidationRules));
    }
}

public class TestStepValidationRuleProvider : AbstractValidationRulesProvider<TestStepEntity>
{
    public override List<Func<TestStepEntity, TeczterValidationResult>> GetRules()
    {
        return GetRules(typeof(TestStepValidationRules));
    }
}