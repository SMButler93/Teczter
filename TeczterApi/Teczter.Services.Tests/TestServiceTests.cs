using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Infrastructure.Cache;
using Teczter.Services.AdapterInterfaces;
using Teczter.Services.RequestDtos.TestSteps;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.Tests;

[TestFixture]
public class TestServiceTests
{
    private readonly Mock<ITestAdapter> _testAdapterMock = new();
    private readonly Mock<IExecutionAdapter> _executionAdapterMock = new();
    private readonly Mock<ITestComposer> _testComposerMock = new();
    private readonly UnitOfWorkFake _uow = new();
    private readonly Mock<IValidator<TestEntity>> _testValidatorMock = new();
    private readonly Mock<ITeczterCache<TestEntity>> _cache = new();

    private TestService _sut = null!;

    [SetUp]
    public void Setup()
    {
        _sut = new TestService(
            _testAdapterMock.Object,
            _executionAdapterMock.Object,
            _testComposerMock.Object,
            _uow,
            _testValidatorMock.Object,
            _cache.Object);
    }

    [Test]
    public async Task RemoveLinkUrl_WhenPresent_ShouldRemove()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        var validationResult = new ValidationResult();
        const string url = "www.url.com";
        var ct = new CancellationTokenSource().Token;
        test.AddLinkUrl(url);
        _testValidatorMock.Setup(x => x.ValidateAsync(test, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        //Act:
        var result = await _sut.RemoveLinkUrl(test, url, ct);

        //Assert.
        result.Value.ShouldNotBeNull();
        test.Urls.ShouldNotContain(url);
    }

    [Test]
    public async Task RemoveLinkUrl_WhenLinkUrlDoesNotExist_ShouldNotAlterCollection()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        test.AddLinkUrl("www.url.com");
        const string urlToRemove = "www.url2.com";
        List<string> currentUrls = [.. test.Urls];

        var validationResult = new ValidationResult();

        _testValidatorMock.Setup(x => x.ValidateAsync(test, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        //Act:
        await _sut.RemoveLinkUrl(test, urlToRemove, CancellationToken.None);

        //Assert:
        test.Urls.Count.ShouldBe(currentUrls.Count);
    }

    [Test]
    public async Task RemoveTestStep_WhenTestStepIsRemovedFromTest_ShouldRemove()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        var stepToRemove = test.TestSteps[0];
        var validationResult = new ValidationResult();
        var ct = new CancellationTokenSource().Token;
        _testValidatorMock.Setup(x => x.ValidateAsync(test, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        //Act:
        var result = await _sut.RemoveTestStep(test, stepToRemove.Id, ct);

        //Assert:
        result.Value.ShouldNotBeNull();
        test.TestSteps.Count.ShouldBe(3);
        test.TestSteps.ShouldNotContain(stepToRemove);
        stepToRemove.IsDeleted.ShouldBeTrue();
    }

    [Test]
    public async Task RemoveTestStep_WhenTheTestStepDoesNotExist_ShouldNOtFailOrThrow()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        const int stepToRemoveId = 5;
        var validationResult = new ValidationResult();
        
        _testValidatorMock.Setup(x => x.ValidateAsync(test, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        //Act:
        var result = await _sut.RemoveTestStep(test, stepToRemoveId, CancellationToken.None);

        //Assert:
        test.TestSteps.SingleOrDefault(x => x.Id == stepToRemoveId).ShouldBeNull();
        result.Value.ShouldNotBeNull();
        result.ErrorMessages.ShouldBeEmpty();
    }

    [Test]
    public async Task UpdateTestStep_WhenStepPlacementIsUpdated_ShouldCorrectlyOrderSteps()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        var validationResult = new ValidationResult();
        var stepToUpdate = test.TestSteps.Single(x => x.StepPlacement == 1);
        var request = new UpdateTestStepRequestDto { StepPlacement = 4, };
        var ct = new CancellationTokenSource().Token;
        _testValidatorMock.Setup(x => x.ValidateAsync(test, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        //Act:
        var result = await _sut.UpdateTestStep(test, stepToUpdate.Id, request, ct);

        //Assert:
        result.Value.ShouldNotBeNull();
        stepToUpdate.StepPlacement.ShouldBe(4);
        test.TestSteps.Count.ShouldBe(4);
    }

    private static TestEntity GetBasicTestInstance()
    {
        return new TestEntity()
        {
            Id = 1,
            IsDeleted = false,
            Title = "Basic test instance.",
            Description = "A basic instance for testing.",
            OwningDepartment = Department.Accounting,
            TestSteps = GetBasicTestStepInstances()
        };
    }

    private static List<TestStepEntity> GetBasicTestStepInstances()
    {
        return
        [
            new TestStepEntity()
            {
                Id = 1,
                StepPlacement = 1,
                Instructions = "Step one."
            },
            new TestStepEntity()
            {
                Id = 2,
                StepPlacement = 2,
                Instructions = "Step two."
            },
            new TestStepEntity()
            {
                Id = 3,
                StepPlacement = 3,
                Instructions = "Step three."
            },
            new TestStepEntity()
            {
                Id = 4,
                StepPlacement = 4,
                Instructions = "Step four."
            }
        ];
    }

    private static List<TestEntity> GetMultipleBasicTestInstances()
    {
        return
        [
            new TestEntity()
            {
                Title = "One",
                OwningDepartment = Department.Accounting
            },
            new TestEntity()
            {
                Title = "Two",
                OwningDepartment = Department.Core
            },
            new TestEntity()
            {
                Title = "Three",
                OwningDepartment = Department.Operations
            },
            new TestEntity()
            {
                Title = "Four",
                OwningDepartment = Department.Trading
            }
        ];
    }
}
