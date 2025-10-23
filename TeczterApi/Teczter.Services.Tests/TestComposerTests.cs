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
    public void AddLinkUrl_ValidateInvariants_WhenValidAndAdded_ShouldSucceed_And_ExistInTheInstanceProvided()
    {
        //Arrange:
        var linkUrl = "www.ValidUrl.com";

        //Act:
        var result = _sut.AddLinkUrl(linkUrl).ValidateInvariants();

        //Assert:
        result.Value.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
        result.Value.Urls.ShouldContain(linkUrl);
    }

    [Test]
    public void AddMultipleLinkUrls_ValidateINvariants_WhenValidAndAdded_ShouldSuccesed_And_ExistInTheInstanceProvided()
    {
        //Arrange:
        List<string> linkUrl = ["www.ValidUrl.com", "www.ValidUrl2.com"];

        //Act:
        var result = _sut.AddLinkUrls(linkUrl).ValidateInvariants();

        //Assert:
        result.Value.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
        result.Value.Urls.ShouldBe(linkUrl);
    }

    [Test]
    public void AddStep_ValidateInvariants_WhenAdded_ShouldSucceeed_And_ExistInTheInstanceProvided()
    {
        //Arrange:
        var testStep = GetSpecifiedNumberOfBasicTestSteps(1).Single();

        //Act:
        var result = _sut.AddStep(testStep).ValidateInvariants();

        //Assert:
        result.Value.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
        result.Value.TestSteps.ShouldContain(testStep);
    }

    [Test]
    public void AddSteps_ValidateInvariants__WhenMultipleStepsAdded_ShouldSucceed_And_AllExistInTheInstanceProvided()
    {
        //Arrange:
        var testSteps = GetSpecifiedNumberOfBasicTestSteps(4);

        //Act:
        var result = _sut.AddSteps(testSteps).ValidateInvariants();

        //Assert:
        result.Value.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
        result.Value.TestSteps.ShouldBe(testSteps);
    }

    [Test]
    public void AddStep_ValidateINvariants_WhenAddedFromRequestDto_ShouldSucceed_And_ExistInTheInstanceProvided()
    {
        //Arrange:
        var testStep = GetSpecifiedNumberOfBasicCreateTestStepRequestDtos(1).Single();

        //Act:
        var result = _sut.AddStep(testStep).ValidateInvariants();

        //Assert:
        result.Value.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
        result.Value.TestSteps.Single().StepPlacement.ShouldBe(testStep.StepPlacement);
        result.Value.TestSteps.Single().Instructions.ShouldBe(testStep.Instructions);
    }

    [Test]
    public void AddSteps_ValidateINvariants_WhenMultipleStepsAddedFromRequestDtos_ShouldSucceed_And_AllExistInTheInstanceProvided()
    {
        //Arrange:
        var testSteps = GetSpecifiedNumberOfBasicCreateTestStepRequestDtos(2);

        //Act:
        var result = _sut.AddSteps(testSteps).ValidateInvariants();

        //Assert:
        result.Value.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
        result.Value.TestSteps.Count.ShouldBe(testSteps.Count);
        result.Value.TestSteps.First().StepPlacement.ShouldBe(testSteps.First().StepPlacement);
        result.Value.TestSteps.First().Instructions.ShouldBe(testSteps.First().Instructions);
        result.Value.TestSteps.Last().StepPlacement.ShouldBe(testSteps.Last().StepPlacement);
        result.Value.TestSteps.Last().Instructions.ShouldBe(testSteps.Last().Instructions);
    }

    [Test]
    public void SetDescription_ValidateInvariants_WhenInvoked_ShouldSucceed_And_SetDescriptionAppropriately()
    {
        //Arrange:
        var description = "New description for test";

        //Act:
        var result = _sut.SetDescription(description).ValidateInvariants();

        //Assert:
        result.Value.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
        result.Value.Description.ShouldBe(description);
    }

    [Test]
    public void SetDescription_ValidateINvariants_WhenNullValue_ShouldSucceed_And_LeaveAsPreviouslySetValue()
    {
        //Arrange:
        var originaltest = GetBasicSingleTestInstance();
        var currentDescription = originaltest.Description;

        //Act:
        var result = _sut.SetDescription(null).ValidateInvariants();

        //Assert:
        result.Value.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
        result.Value.Description.ShouldNotBeNull();
        result.Value.Description.ShouldBe(currentDescription);
    }

    [Test]
    public void SetOwningDepartment_ValidateINvariants_WhenInvoked_ShouldSucceed_And_SetOwningDepartmentAppropriately()
    {
        //Arrange:
        var department = "Accounting";

        //Act:
        var result = _sut.SetOwningDepartment(department).ValidateInvariants();

        //Assert:
        result.Value.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
        result.Value.OwningDepartment.ShouldBe(Department.Accounting);
    }

    [Test]
    public void SetOwningDepartment_ValidateInvariants_WhenInvalidDepartment_ShouldFail()
    {
        //Arrange:
        var department = "Invalid dept";

        //Act:
        var result = _sut.SetOwningDepartment(department).ValidateInvariants();

        //Assert:
        result.Value.ShouldBeNull();
        result.IsValid.ShouldBeFalse();
        result.ErrorMessages.ShouldNotBeEmpty();
    }

    [Test]
    public void SetOwningDepartment_InvalidateINvariants_WhenNullValue_ShouldSucceed_And_LeaveAsPreviouslySetValue()
    {
        //Arrange:
        var originalTest = GetBasicSingleTestInstance();
        var currentDepartment = originalTest.OwningDepartment;

        //Act:
        var result = _sut.SetOwningDepartment(null).Build();

        //Assert:
        result.Value.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
        result.Value.OwningDepartment.ShouldBe(currentDepartment);
    }

    [Test]
    public void SetTitle_ValidateInvariants_WhenInvoked_ShouldSucceed_And_SetTitleAppropriately()
    {
        //Arrange:
        var title = "New Title";

        //Act:
        var result = _sut.SetTitle(title).ValidateInvariants();

        //Assert:
        result.Value.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
        result.Value.Title.ShouldBe(title);
    }

    [Test]
    public void SetTitle_ValidateInvariants_WhenNullValue_ShouldSucceed_LeaveAsPreviouslySetValue()
    {
        //Arrange:
        var originalTest = GetBasicSingleTestInstance();
        var currentTitle = originalTest.Title;

        //Act:
        var result = _sut.SetTitle(null).Build();

        //Assert:
        result.Value.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
        result.Value.Title.ShouldNotBeNull();
        result.Value.Title.ShouldBe(currentTitle);
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
