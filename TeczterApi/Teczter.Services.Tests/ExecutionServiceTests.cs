using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Services.AdapterInterfaces;
using Teczter.Services.RequestDtos.Executions;

namespace Teczter.Services.Tests;

[TestFixture]
public class ExecutionServiceTests
{
    private Mock<IExecutionAdapter> _executionAdapterMock = new();
    private Mock<IValidator<ExecutionEntity>> _executionValidatorMock = new();
    private UnitOfWorkFake _uow = new();

    private ExecutionService _sut = null!;

    [SetUp]
    public void Setup()
    {
        _sut = new ExecutionService(
            _executionAdapterMock.Object,
            _uow,
            _executionValidatorMock.Object);
    }

    [Test]
    public async Task CompleteExecution_WhenExecutionReturnedAndRequestIsAPass_ShouldPassExecution()
    {
        //Arrange:
        var request = GetCompletionRequest();
        var executionId = 1;
        var execution = GetExecutionInstance();
        var validationResult = new ValidationResult();
        _executionAdapterMock.Setup(x => x.GetExecutionById(executionId)).ReturnsAsync(execution);
        _executionValidatorMock.Setup(x => x.ValidateAsync(execution, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        //Act:
        var result = await _sut.CompleteExecution(execution, request);

        //Assert:
        result.IsValid.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.ExecutionState.ShouldBe(ExecutionStateType.Pass);
    }

    private static CompleteExecutionRequestDto GetCompletionRequest()
    {
        return new CompleteExecutionRequestDto()
        {
            HasPassed = true
        };
    }

    private static ExecutionEntity GetExecutionInstance()
    {
        return new ExecutionEntity()
        {
            Id = 1
        };
    }
}
