using FluentValidation;
using FluentValidation.Results;
using MockQueryable;
using Moq;
using NUnit.Framework;
using Shouldly;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Domain.Exceptions;
using Teczter.Services.RequestDtos;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.Tests;

[TestFixture]
public class TestServiceTests
{
    private readonly Mock<ITestAdapter> _testAdapterMock = new();
    private readonly Mock<IExecutionAdapter> _executionAdapterMock = new();
    private readonly Mock<ITestBuilder> _testBuilderMock = new();
    private readonly UnitOfWorkFake _uow = new();
    private readonly Mock<IValidator<TestEntity>> _testValidatorMock = new();

    private TestService _sut = null!;

    [SetUp]
    public void Setup()
    {
        _sut = new TestService(
            _testAdapterMock.Object,
            _executionAdapterMock.Object,
            _testBuilderMock.Object,
            _uow,
            _testValidatorMock.Object);
    }

    [Test]
    public async Task ValidateTestState_WhenValid_ShouldReturnPass()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        var validationResult = new ValidationResult();
        _testValidatorMock.Setup(x => x.ValidateAsync(test, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        //Act:
        var result = await _sut.ValidateTestState(test);

        //Assert:
        result.IsValid.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
    }

    [Test]
    public async Task ValidateTestState_WhenNotValid_ShouldReturnFail()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        var validationResult = new ValidationResult { Errors = [new ValidationFailure()] };
        _testValidatorMock.Setup(x => x.ValidateAsync(test, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        //Act:
        var result = await _sut.ValidateTestState(test);

        //Assert:
        result.IsValid.ShouldBeFalse();
        result.Value.ShouldBeNull();
    }

    [Test]
    public async Task DeleteTest_WhenDeleted_ShouldSetIsDeletedToTrue()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        List<ExecutionEntity> executionsSearchResult = [];
        _executionAdapterMock.Setup(x => x.GetExecutionsForTest(test.Id)).ReturnsAsync(executionsSearchResult);

        //Act:
        await _sut.DeleteTest(test);

        //Assert
        test.IsDeleted.ShouldBeTrue();
    }

    [Test]
    public async Task GetTestSearchResults_WhenNoFiltersApplied_ShouldGetAllInstances()
    {
        //Arrange:
        var tests = GetMultipleBasicTestInstances();
        var queryable = tests.AsQueryable();
        _testAdapterMock.Setup(x => x.GetBasicTestSearchBaseQuery()).Returns(queryable.BuildMock());

        //Act:
        var results = await _sut.GetTestSearchResults(null, null);

        //Assert:
        results.ShouldBe(tests);
    }

    [Test]
    public async Task GetTestSearchResults_WhenFilteredOnTestName_ShouldGetInstancesThatMatch()
    {
        //Arrange:
        var tests = GetMultipleBasicTestInstances();
        var queryable = tests.AsQueryable();
        _testAdapterMock.Setup(x => x.GetBasicTestSearchBaseQuery()).Returns(queryable.BuildMock());

        //Act:
        var results = await _sut.GetTestSearchResults("One", null);

        //Assert:
        results.Count.ShouldBe(1);
        results[0].ShouldBe(tests[0]);
    }

    [Test]
    public async Task GetTestSearchResults_WhenFilteredOnOwningDepartment_ShouldGetInstancesThatMatch()
    {
        //Arrange:
        var tests = GetMultipleBasicTestInstances();
        var queryable = tests.AsQueryable();
        _testAdapterMock.Setup(x => x.GetBasicTestSearchBaseQuery()).Returns(queryable.BuildMock());

        //Act:
        var results = await _sut.GetTestSearchResults(null, Department.Accounting.ToString());

        //Assert:
        results.Count.ShouldBe(1);
        results[0].ShouldBe(tests[0]);
    }

    [Test]
    public async Task GetTestSearchResults_WhenFilteredWithNoMatches_ShouldReturnEmptyList()
    {
        //Arrange:
        var tests = GetMultipleBasicTestInstances();
        var queryable = tests.AsQueryable();
        _testAdapterMock.Setup(x => x.GetBasicTestSearchBaseQuery()).Returns(queryable.BuildMock());

        //Act:
        var results = await _sut.GetTestSearchResults("ABC", null);

        //Assert:
        results.ShouldBeEmpty();
    }

    [Test]
    public async Task RemoveLinkUrl_WhenPresent_ShouldRemove()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        var validationResult = new ValidationResult();
        var url = "www.url.com";
        test.Urls.Add(url);
        _testValidatorMock.Setup(x => x.ValidateAsync(test, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        //Act:
        var result = await _sut.RemoveLinkUrl(test, url);

        //Assert.
        result.Value.ShouldNotBeNull();
        test.Urls.ShouldNotContain(url);
    }

    [Test]
    public void RemoveLinkUrl_WhenLinkUrlDoesNotExist_ShouldThrowTeczterValidationException()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        var url = "www.url.com";

        //Act & Assert:
        Should.Throw<TeczterValidationException>(() => _sut.RemoveLinkUrl(test, url));
    }

    [Test]
    public async Task RemoveTestStep_WhenTestStepIsRemovedFromTest_ShouldRemove()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        var stepToRemove = test.TestSteps[0];
        var validationResult = new ValidationResult();
        _testValidatorMock.Setup(x => x.ValidateAsync(test, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        //Act:
        var result = await _sut.RemoveTestStep(test, stepToRemove.Id);

        //Assert:
        result.Value.ShouldNotBeNull();
        test.TestSteps.Count.ShouldBe(3);
        test.TestSteps.ShouldNotContain(stepToRemove);
        stepToRemove.IsDeleted.ShouldBeTrue();
    }

    [Test]
    public void RemoveTestStep_WhenTheTestStepDoesNotExist_ShouldThrowTecztervalidationException()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        var stepToRemoveId = 5;

        //Act & Assert:
        Should.Throw<TeczterValidationException>(() => _sut.RemoveTestStep(test, stepToRemoveId));
    }

    [Test]
    public async Task UpdateTestStep_WhenStepPlacementIsUpdated_ShouldCorrectlyOrderSteps()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        var validationresult = new ValidationResult();
        var stepToUpdate = test.TestSteps.Single(x => x.StepPlacement == 1);
        var request = new UpdateTestStepRequestDto { StepPlacement = 4, };
        _testValidatorMock.Setup(x => x.ValidateAsync(test, It.IsAny<CancellationToken>())).ReturnsAsync(validationresult);

        //Act:
        var result = await _sut.UpdateTestStep(test, stepToUpdate.Id, request);

        //Assert:
        result.Value.ShouldNotBeNull();
        stepToUpdate.StepPlacement.ShouldBe(4);
        test.TestSteps.Count.ShouldBe(4);
    }

    [Test]
    public void UpdateTestStep_WhenTestStepDoesNotExist_ShouldThrowTeczterValidationresult()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        var stepToUpdate = new TestStepEntity { Id = 5, StepPlacement = 5, Instructions = "step 5." };
        var request = new UpdateTestStepRequestDto { StepPlacement = 4, };

        //Act & Assert:
        Should.Throw<TeczterValidationException>(() => _sut.UpdateTestStep(test, stepToUpdate.Id, request));
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
