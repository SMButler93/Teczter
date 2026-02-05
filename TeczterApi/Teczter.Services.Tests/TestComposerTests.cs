using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Services.ComposersAndBuilders;
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
    public void AddLinkUrl_ShouldSucceed_And_ExistInTheCollection()
    {
        //Arrange:
        var linkUrl = "www.ValidUrl.com";

        //Act:
        var result = _sut.AddLinkUrl(linkUrl).Build();

        //Assert:
        result.Urls.ShouldContain(linkUrl);
    }

    [Test]
    public void AddLinkUrls_ShouldSucceed_And_AllExistInTheCollection()
    {
        //Arrange:
        List<string> linkUrls = ["www.ValidUrl.com", "www.ValidUrl2.com"];

        //Act:
        var result = _sut.AddLinkUrls(linkUrls).Build();

        //Assert:
        result.Urls.ShouldBe(linkUrls);
    }

    [Test]
    public void AddStep_ShouldSucceed_And_ExistInTheTestInstanceProvided()
    {
        //Arrange:
        var testStep = GetSpecifiedNumberOfBasicTestSteps(1).Single();

        //Act:
        var test = _sut.AddStep(testStep).Build();

        //Assert:
        test.ShouldNotBeNull();
        test.TestSteps.ShouldContain(testStep);
    }

    [Test]
    public void AddSteps_WhenMultipleStepsAdded_ShouldSucceed_And_AllExistInTheTestInstanceProvided()
    {
        //Arrange:
        var testSteps = GetSpecifiedNumberOfBasicTestSteps(4);

        //Act:
        var test = _sut.AddSteps(testSteps).Build();

        //Assert:
        test.ShouldNotBeNull();
        test.TestSteps.ShouldBe(testSteps);
    }

    [Test]
    public void SetDescription_ShouldSucceed_And_SetDescriptionAppropriately()
    {
        //Arrange:
        const string description = "New description for test";

        //Act:
        var test = _sut.SetDescription(description).Build();

        //Assert:
        test.ShouldNotBeNull();
        test.Description.ShouldBe(description);
    }

    [Test]
    public void SetOwningDepartment_ShouldSucceed_And_SetOwningDepartmentAppropriately()
    {
        //Arrange:
        const string department = "Accounting";

        //Act:
        var test = _sut.SetOwningDepartment(department).Build();

        //Assert:
        test.ShouldNotBeNull();
        test.OwningDepartment.ShouldBe(Department.Accounting);
    }

    [Test]
    public void SetOwningDepartment_WhenInvalidDepartment_ShouldThrow()
    {
        //Arrange:
        const string department = "Invalid dept";

        //Act and Assert:
        Should.Throw<ArgumentException>(() => _sut.SetOwningDepartment(department).Build());
    }

    [Test]
    public void SetTitle_ShouldSetTitleAppropriately()
    {
        //Arrange:
        const string title = "New Title";

        //Act:
        var test = _sut.SetTitle(title).Build();

        //Assert:
        test.ShouldNotBeNull();
        test.Title.ShouldBe(title);
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
