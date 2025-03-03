using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;

namespace Teczter.Domain.Tests.TestEntityTests;

[TestFixture]
public class TestEntityTests
{
    public TestEntity GetSubjectUnderTest()
    {
        return new TestEntity()
        {
            IsDeleted = false,
            Title = "Basic test instance.",
            Description = "Basic instance for testing.",
            OwningDepartment = "Operations",
            TestSteps = GetBasicTestStepInstances()
        };
    }

    [Test]
    public void RemoveTestStep_WhenRemoved_ShouldOrderRemainingStepsCorrectly()
    {
        //Arrange
        var sut = GetSubjectUnderTest();

        //Act:
        sut.RemoveTestStep(sut.TestSteps[0]);

        //Assert:
        sut.TestSteps.Count.ShouldBe(3);
        sut.TestSteps[0].StepPlacement.ShouldBe(1);
        sut.TestSteps[1].StepPlacement.ShouldBe(2);
        sut.TestSteps[2].StepPlacement.ShouldBe(3);
    }

    private List<TestStepEntity> GetBasicTestStepInstances()
    {
        return
        [
            new TestStepEntity()
            {
                StepPlacement = 1,
                Instructions = "Step one."
            },
            new TestStepEntity()
            {
                StepPlacement = 2,
                Instructions = "Step two."
            },
            new TestStepEntity()
            {
                StepPlacement = 3,
                Instructions = "Step three."
            },
            new TestStepEntity()
            {
                StepPlacement = 4,
                Instructions = "Step four."
            }
        ];
    }
}
