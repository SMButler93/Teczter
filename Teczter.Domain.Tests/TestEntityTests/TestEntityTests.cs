using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Domain.Exceptions;

namespace Teczter.Domain.Tests.TestEntityTests;

[TestFixture]
public class TestEntityTests
{
    public static TestEntity GetSubjectUnderTest()
    {
        return TestInstanceProvider.GetBasicTestInstanceWithNumberOfTestSteps(4);
    }

    [Test]
    public void RemoveTestStep_WhenRemovedFromBeginning_ShouldOrderRemainingStepsCorrectly()
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

    [Test]
    public void AddTestStep_WhenAddedInTheMiddle_ShouldAmendStepPlacements()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();
        var newStep = new TestStepEntity
        {
            StepPlacement = 2,
            Instructions = "The new step two."
        };

        //Act:
        sut.AddTestStep(newStep);

        //Assert:
        sut.TestSteps.Count.ShouldBe(5);
        sut.TestSteps[1].Instructions.ShouldBe("The new step two.");
        sut.TestSteps[0].StepPlacement.ShouldBe(1);
        sut.TestSteps[1].StepPlacement.ShouldBe(2);
        sut.TestSteps[2].StepPlacement.ShouldBe(3);
        sut.TestSteps[3].StepPlacement.ShouldBe(4);
        sut.TestSteps[4].StepPlacement.ShouldBe(5);
    }

    [Test]
    public void Delete_WhenDeleted_ShouldMarkAllTestStepsAsDeleted()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();

        //Act:
        sut.Delete();

        //Assert:
        sut.IsDeleted.ShouldBeTrue();
        sut.TestSteps.ShouldAllBe(x => x.IsDeleted);
    }

    [Test]
    public void AddLinkUrl_WhenInvalid_ShouldThrow()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();
        var invalidUrl = "Invalid";

        //Act&Assert:
        Should.Throw<TeczterValidationException>(() => sut.AddLinkUrl(invalidUrl));
    }

    [Test]
    public void AddLinkUrl_WhenValid_ShouldNotThrow()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();
        var validUrl = "www.NotInvalid.com";

        //Act&Assert:
        Should.NotThrow(() => sut.AddLinkUrl(validUrl));
    }
}
