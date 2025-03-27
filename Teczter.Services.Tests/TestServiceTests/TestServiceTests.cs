using FluentValidation;
using FluentValidation.Results;
using MockQueryable;
using Moq;
using NUnit.Framework;
using Shouldly;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.Tests.TestServiceTests;

[TestFixture]
public class TestServiceTests
{
    private Mock<ITestAdapter> _testAdapterMock = null!;
    private Mock<IExecutionAdapter> _executionAdapterMock = null!;
    private Mock<ITestBuilder> _testBuilderMock = null!;
    private UnitOfWorkFake _uow = null!;
    private Mock<IValidator<TestEntity>> _testValidatorMock = null!;

    private TestService _sut = null!;

    [SetUp]
    public void Setup()
    {
        _testAdapterMock = new Mock<ITestAdapter>();
        _executionAdapterMock = new Mock<IExecutionAdapter>();
        _testBuilderMock = new Mock<ITestBuilder>();
        _uow = new UnitOfWorkFake();
        _testValidatorMock = new Mock<IValidator<TestEntity>>();

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
        var validTest = GetBasicTestInstance();
        var validationResult = new ValidationResult { Errors = [] };
        _testValidatorMock.Setup(x => x.ValidateAsync(validTest, It.IsAny<CancellationToken>())).Returns(Task.FromResult(validationResult));

        //Act:
        var result = await _sut.ValidateTestState(validTest);

        //Assert:
        result.IsValid.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
    }

    [Test]
    public async Task ValidateTestState_WhenNotValid_ShouldReturnFail()
    {
        //Arrange:
        var validTest = GetBasicTestInstance();
        var validationResult = new ValidationResult { Errors = [new ValidationFailure()] };
        _testValidatorMock.Setup(x => x.ValidateAsync(validTest, It.IsAny<CancellationToken>())).Returns(Task.FromResult(validationResult));

        //Act:
        var result = await _sut.ValidateTestState(validTest);

        //Assert:
        result.IsValid.ShouldBeFalse();
        result.Value.ShouldBeNull();
    }

    [Test]
    public async Task DeleteTest_WhenCalled_ShouldSetIsDeletedToTrue()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        List<ExecutionEntity> executionsSearchResult = [];
        _executionAdapterMock.Setup(x => x.GetExecutionsForTest(test.Id)).Returns(Task.FromResult(executionsSearchResult));

        //Act:
        await _sut.DeleteTest(test);

        //Assert
        test.IsDeleted.ShouldBeTrue();
    }

    [Test]
    public async Task GetTestSearchResults_WhenNoFilters_ShouldGetAllInstances()
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
    public async Task GetTestSearchResults_WhenNameFilter_ShouldGetInstancesThatMatch()
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
    public async Task GetTestSearchResults_WhenOwningDepartmentFilter_ShouldGetInstancesThatMatch()
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
    public async Task GetTestSearchResults_WhenFilterWithNoMatches_ShouldReturnEmptyList()
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

    private static TestEntity GetBasicTestInstance()
    {
        return new TestEntity()
        {
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
