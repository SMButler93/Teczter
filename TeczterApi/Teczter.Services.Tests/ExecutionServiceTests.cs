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
    private readonly Mock<IExecutionAdapter> _executionAdapterMock = new();
    private readonly Mock<IValidator<ExecutionEntity>> _executionValidatorMock = new();
    private readonly UnitOfWorkFake _uow = new();

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
        var execution = GetExecutionInstance();
        var validationResult = new ValidationResult();
        var ct = new CancellationTokenSource().Token;
        _executionValidatorMock.Setup(x => x.ValidateAsync(execution, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
        _executionAdapterMock.Setup(x => x.GetExecutionById(execution.Id,  It.IsAny<CancellationToken>())).ReturnsAsync(execution);

        //Act:
        var result = await _sut.CompleteExecution(execution, request, ct);

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
            Id = 1,
            ExecutionGroup = new(){Id = 1},
        };
    }
}
