using Teczter.Domain.Entities;
using Teczter.Domain.Enums;

namespace Teczter.Domain.ValueObjects;

public record TestState
{
    public TestStateTypes Result { get; private set; }
    public bool IsAPass => Result == TestStateTypes.Pass;
    public bool IsUntested => Result == TestStateTypes.Untested;

    public TestState()
    {
        Result = TestStateTypes.Untested;
    }

    public TestState(List<TestStepEntity> testSteps)
    {
        AssessTestState(testSteps);
    }

    public void AssessTestState(List<TestStepEntity> testSteps)
    {
        if (!testSteps.Any() || testSteps.Any(x => x.IsUntested))
        {
            Result = TestStateTypes.Untested;
        }
        else if (testSteps.All(x => x.HasPassed))
        {
            Result = TestStateTypes.Pass;
        }
        else
        {
            Result = TestStateTypes.Fail;
        }
    }

    public void Pass()
    {
        Result = TestStateTypes.Pass;
    }

    public void Fail()
    {
        Result = TestStateTypes.Fail;
    }

    public void Reset()
    {
        Result = TestStateTypes.Untested;
    }
}