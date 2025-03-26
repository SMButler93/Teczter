using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Domain.Tests.TestStepEntityTests;

namespace Teczter.Domain.Tests.TestEntityTests;

public static class TestInstanceProvider
{
    public static TestEntity GetBasicTestInstanceWithNumberOfTestSteps(int numberOftestSteps)
    {
        return new TestEntity()
        {
            IsDeleted = false,
            Title = "Basic test instance.",
            Description = "Basic instance for testing.",
            OwningDepartment = Department.Operations,
            TestSteps = TestStepInstanceProvider.GetMultipleBasicTestStepInstances(numberOftestSteps)
        };
    }
}
