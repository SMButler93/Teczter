using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;

namespace Teczter.Domain.Tests;

[TestFixture]
public class TestEntityTests
{
    private TestEntity _sut = null!;

    [SetUp]
    public void Setup()
    {
        _sut = new TestEntity()
        {
            IsDeleted = false,
            Title = "Basic test instance.",
            Description = "Basic instance for testing.",
            OwningDepartment = Department.Operations,
            TestSteps = GetMultipleBasicTestStepInstances(4)
        };
    }

    [Test]
    public void RemoveTestStep_WhenRemovedFromBeginning_ShouldOrderRemainingStepsCorrectly()
    {
        //Act:
        _sut.RemoveTestStep(_sut.TestSteps[0].Id);

        //Assert:
        _sut.TestSteps.Count.ShouldBe(3);
        _sut.TestSteps[0].StepPlacement.ShouldBe(1);
        _sut.TestSteps[1].StepPlacement.ShouldBe(2);
        _sut.TestSteps[2].StepPlacement.ShouldBe(3);
    }

    [Test]
    public void AddTestStep_WhenAddedInTheMiddle_ShouldAmendStepPlacements()
    {
        //Arrange:
        var newStep = new TestStepEntity
        {
            StepPlacement = 2,
            Instructions = "The new step two."
        };

        //Act:
        _sut.AddTestStep(newStep);

        //Assert:
        _sut.TestSteps.Count.ShouldBe(5);
        _sut.TestSteps[0].StepPlacement.ShouldBe(1);
        _sut.TestSteps[1].StepPlacement.ShouldBe(2);
        _sut.TestSteps[1].Instructions.ShouldBe("The new step two.");
        _sut.TestSteps[2].StepPlacement.ShouldBe(3);
        _sut.TestSteps[3].StepPlacement.ShouldBe(4);
        _sut.TestSteps[4].StepPlacement.ShouldBe(5);
    }

    [Test]
    public void Delete_WhenDeleted_ShouldMarkAllTestStepsAsDeleted()
    {
        //Act:
        _sut.Delete();

        //Assert:
        _sut.IsDeleted.ShouldBeTrue();
        _sut.TestSteps.ShouldAllBe(x => x.IsDeleted);
    }

    [Test]
    public void AddLinkUrl_ShouldExistOnTestInstance()
    {
        //Arrange:
        const string validUrl = "www.validUrl.com";
        //Act:
        _sut.AddLinkUrl(validUrl);

        //Assert:
        _sut.Urls.ShouldContain(validUrl);
    }

    [Test]
    public void RemoveLinkUrl_WhenExists_ShouldRemove()
    {
        //Arrange:
        const string newUrl = "www.validUrl.com";
        _sut.AddLinkUrl(newUrl);

        //Act:
        _sut.RemoveLinkUrl(newUrl);

        //Assert:
        _sut.Urls.ShouldNotContain(newUrl);
    }

    [Test]
    public void OrderTestSteps_WhenCalled_ShouldOrderTestStepsByStepPlacement()
    {
        //Arrange:
        var unorderedSteps = GetUnorderedTestStepInstances();
        _sut.TestSteps = unorderedSteps;

        //Act:
        _sut.OrderTestSteps();

        //Assert:
        _sut.TestSteps[0].StepPlacement.ShouldBe(1);
        _sut.TestSteps[1].StepPlacement.ShouldBe(2);
        _sut.TestSteps[2].StepPlacement.ShouldBe(3);
        _sut.TestSteps[3].StepPlacement.ShouldBe(4);
    }

    [Test]
    public void SetCorrectStepPlacementValuesOnUpdate_WhenStepUpdated_ShouldEnsureAlltestStepsAreOrderedAccordingly()
    {
        //Arrange:
        var stepToUpdate = _sut.TestSteps.Single(x => x.StepPlacement == 1);
        stepToUpdate.Update(2, null, []);

        //Act:
        _sut.SetCorrectStepPlacementValuesOnUpdate();

        //Assert:
        _sut.TestSteps[0].StepPlacement.ShouldBe(1);
        _sut.TestSteps[1].StepPlacement.ShouldBe(2);
        _sut.TestSteps[1].Instructions.ShouldBe(stepToUpdate.Instructions);
        _sut.TestSteps[2].StepPlacement.ShouldBe(3);
        _sut.TestSteps[3].StepPlacement.ShouldBe(4);
    }

    private static List<TestStepEntity> GetMultipleBasicTestStepInstances(int numberOfInstances)
    {
        var steps = new List<TestStepEntity>();

        for (var i = 1; i <= numberOfInstances; i++)
        {
            var step = new TestStepEntity
            {
                Id = i,
                StepPlacement = i,
                Instructions = "Step" + i.ToString()
            };

            steps.Add(step);
        }

        return steps;
    }

    private static List<TestStepEntity> GetUnorderedTestStepInstances()
    {
        return
        [
            new TestStepEntity
            {
                StepPlacement = 3,
                Instructions = "Step 3"
            },
            new TestStepEntity
            {
                StepPlacement = 2,
                Instructions = "Step 2"
            },
            new TestStepEntity
            {
                StepPlacement = 4,
                Instructions = "Step 4"
            },
            new TestStepEntity
            {
                StepPlacement = 1,
                Instructions = "Step 1"
            }
        ];
    }
}
