using Teczter.Domain.Entities;

namespace Teczter.Domain.Tests.TestStepEntityTests;

public static class TestStepInstanceProvider
{
    public static List<TestStepEntity> GetMultipleBasicTestStepInstances(int numberOfInstances)
    {
        var steps = new List<TestStepEntity>();

        for (var i = 1; i <= numberOfInstances; i++)
        {
            var step = new TestStepEntity
            {
                StepPlacement = i,
                Instructions = "Step" + i.ToString()
            };

            steps.Add(step);
        }

        return steps;
    }

    public static TestStepEntity GetBasicTestStepInstance()
    {
        return new TestStepEntity
        {
            StepPlacement = 1,
            Instructions = "Step 1"
        };
    }
}
