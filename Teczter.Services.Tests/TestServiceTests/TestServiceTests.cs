using FluentValidation;
using FluentValidation.Results;
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
    private readonly Mock<ITestAdapter> _testAdapterMock;
    private readonly Mock<IExecutionAdapter> _executionAdapterMock;
    private readonly Mock<ITestBuilder> _testBuilderMock;
    private readonly UnitOfWorkFake _uow;
    private readonly Mock<IValidator<TestEntity>> _testValidatorMock;

    public TestServiceTests()
    {
        _testAdapterMock = new Mock<ITestAdapter>();
        _executionAdapterMock = new Mock<IExecutionAdapter>();
        _testBuilderMock = new Mock<ITestBuilder>();
        _uow = new UnitOfWorkFake();
        _testValidatorMock = new Mock<IValidator<TestEntity>>();
    }

    private ITestService GetSubjectUnderTest()
    {
        return new TestService(
            _testAdapterMock.Object,
            _executionAdapterMock.Object,
            _testBuilderMock.Object,
            _uow,
            _testValidatorMock.Object
            );
    }

    [Test]
    public async Task ValidateTestState_WhenValid_ShouldReturnPass()
    {
        //Arrange:
        var validTest = GetBasicTestInstance();
        var sut = GetSubjectUnderTest();
        var validationResult = new ValidationResult { Errors = [] };

        _testValidatorMock.Setup(x => x.ValidateAsync(validTest, It.IsAny<CancellationToken>())).Returns(Task.FromResult(validationResult));

        //Act:
        var result = await sut.ValidateTestState(validTest);

        //Assert:
        result.IsValid.ShouldBeTrue();
    }

    [Test]
    public async Task ValidateTestState_WhenNotValid_ShouldReturnFail()
    {
        //Arrange:
        var validTest = GetBasicTestInstance();
        var sut = GetSubjectUnderTest();
        var validationResult = new ValidationResult { Errors = [new ValidationFailure()] };

        _testValidatorMock.Setup(x => x.ValidateAsync(validTest, It.IsAny<CancellationToken>())).Returns(Task.FromResult(validationResult));

        //Act:
        var result = await sut.ValidateTestState(validTest);

        //Assert:
        result.IsValid.ShouldBeFalse();
    }

    [Test]
    public void DeleteTest_WhenCalled_ShouldSetIsDeletedToTrue()
    {
        //Arrange:
        var test = GetBasicTestInstance();
        var sut = GetSubjectUnderTest();

        //Act:
        var result = sut.DeleteTest(test);

        //Assert
        test.IsDeleted.ShouldBeTrue();
    }

    private TestEntity GetBasicTestInstance()
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
