using Moq;
using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Domain.Exceptions;
using Teczter.Services.Composers;
using Teczter.Services.RequestDtos.TestSteps;

namespace Teczter.Services.Tests;

[TestFixture]
public class TestComposerTests
{
    private TestComposer _sut = null!;

    [SetUp]
    public void Setup()
    {
        _sut = new();
        _sut.UsingContext(GetBasicSingleTestInstance());
    }

    [Test]
    public void AddLinkUrl_WhenValidAndAdded_ShouldExistInTheInstanceProvided()
    {
        //Arrange:
        var linkUrl = "www.ValidUrl.com";

        //Act:
        var result = _sut.AddLinkUrl(linkUrl).Build();

        //Assert:
        result.Urls.ShouldContain(linkUrl);
    }

    [Test]
    public void AddMultipleLinkUrls_WhenValidAndAdded_ShouldExistInTheInstanceProvided()
    {
        //Arrange:
        List<string> linkUrl = ["www.ValidUrl.com", "www.ValidUrl2.com"];

        //Act:
        var result = _sut.AddLinkUrls(linkUrl).Build();

        //Assert:
        result.Urls.ShouldBe(linkUrl);
    }

    [Test]
    public void AddStep_WhenAdded_ShouldExistInTheInstanceProvided()
    {
        //Arrange:
        var testStep = GetSpecifiedNumberOfBasicTestSteps(1).Single();

        //Act:
        var result = _sut.AddStep(testStep).Build();

        //Assert:
        result.TestSteps.ShouldContain(testStep);
    }

    [Test]
    public void AddSteps_WhenMultipleStepsAdded_ShouldAllExistInTheInstanceProvided()
    {
        //Arrange:
        var testSteps = GetSpecifiedNumberOfBasicTestSteps(4);

        //Act:
        var result = _sut.AddSteps(testSteps).Build();

        //Assert:
        result.TestSteps.ShouldBe(testSteps);
    }

    [Test]
    public void AddStep_WhenAddedFromRequestDto_ShouldExistInTheInstanceProvided()
    {
        //Arrange:
        var testStep = GetSpecifiedNumberOfBasicCreateTestStepRequestDtos(1).Single();

        //Act:
        var result = _sut.AddStep(testStep).Build();

        //Assert:
        result.TestSteps.Single().StepPlacement.ShouldBe(testStep.StepPlacement);
        result.TestSteps.Single().Instructions.ShouldBe(testStep.Instructions);
    }

    [Test]
    public void AddSteps_WhenMultipleStepsAddedFromRequestDtos_ShouldAllExistInTheInstanceProvided()
    {
        //Arrange:
        var testSteps = GetSpecifiedNumberOfBasicCreateTestStepRequestDtos(2);

        //Act:
        var result = _sut.AddSteps(testSteps).Build();

        //Assert:
        result.TestSteps.Count.ShouldBe(testSteps.Count);
        result.TestSteps.First().StepPlacement.ShouldBe(testSteps.First().StepPlacement);
        result.TestSteps.First().Instructions.ShouldBe(testSteps.First().Instructions);
        result.TestSteps.Last().StepPlacement.ShouldBe(testSteps.Last().StepPlacement);
        result.TestSteps.Last().Instructions.ShouldBe(testSteps.Last().Instructions);
    }

    [Test]
    public void SetDescription_WhenInvoked_ShouldSetDescriptionAppropriately()
    {
        //Arrange:
        var description = "New description for test";

        //Act:
        var result = _sut.SetDescription(description).Build();

        //Assert:
        result.Description.ShouldBe(description);
    }

    [Test]
    public void SetOwningDepartment_WhenInvoked_ShouldSetOwningDepartmentAppropriately()
    {
        //Arrange:
        var department = "Accounting";

        //Act:
        var result = _sut.SetOwningDepartment(department).Build();

        //Assert:
        result.OwningDepartment.ShouldBe(Department.Accounting);
    }

    [Test]
    public void SetOwningDepartment_WhenInvalidDepartment_ShouldThrowException()
    {
        //Arrange:
        var department = "Invalid dept";

        //Act & Assert:
        Should.Throw<TeczterValidationException>(() => _sut.SetOwningDepartment(department));
    }

    [Test]
    public void SetTitle_WhenInvoked_ShouldSetTitleAppropriately()
    {
        //Arrange:
        var title = "New Title";

        //Act:
        var result = _sut.SetTitle(title).Build();

        //Assert:
        result.Title.ShouldBe(title);
    }

    private static TestEntity GetBasicSingleTestInstance()
    {
        return new TestEntity()
        {
            Id = 1,
            CreatedById = 1,
            RevisedById = 1,
            Title = "Test 1",
            Description = "Test 1 description"
        };
    }

    private static List<TestStepEntity> GetSpecifiedNumberOfBasicTestSteps(int numberOfInstances)
    {
        List<TestStepEntity> steps = [];

        for (var i = 1; i <= numberOfInstances; i++)
        {
            var step = new TestStepEntity()
            {
                Id = i,
                StepPlacement = i,
                Instructions = $"Step {i}"
            };

            steps.Add(step);
        }

        return steps;
    }

    private static List<CreateTestStepRequestDto> GetSpecifiedNumberOfBasicCreateTestStepRequestDtos(int numberOfInstances)
    {
        List<CreateTestStepRequestDto> steps = [];

        for (var i = 1; i <= numberOfInstances; i++)
        {
            var step = new CreateTestStepRequestDto()
            {
                StepPlacement = i,
                Instructions = $"Step {i}"
            };

            steps.Add(step);
        }

        return steps;
    }
}
