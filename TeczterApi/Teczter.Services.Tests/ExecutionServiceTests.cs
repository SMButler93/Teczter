using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Domain.Exceptions;
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
    public async Task CompleteExecution_WhenNullReturned_ShouldThrowTeczterValidationException()
    {
        //Arrange:
        var request = GetCompletionRequest();
        _executionAdapterMock.Setup(x => x.GetExecutionById(It.IsAny<int>())).ReturnsAsync((ExecutionEntity?)null);

        //Act & Assert:
        await Should.ThrowAsync<TeczterValidationException>(() => _sut.CompleteExecution(request));
    }

    [Test]
    public async Task CompleteExecution_WhenExecutionReturnedAndRequestIsAPass_ShouldPassExecution()
    {
        //Arrange:
        var request = GetCompletionRequest();
        var execution = GetExecutionInstance();
        var validationResult = new ValidationResult();
        _executionAdapterMock.Setup(x => x.GetExecutionById(request.ExecutionId)).ReturnsAsync(execution);
        _executionValidatorMock.Setup(x => x.ValidateAsync(execution, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        //Act:
        var result = await _sut.CompleteExecution(request);

        //Assert:
        result.IsValid.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.ExecutionState.ShouldBe(ExecutionStateType.Pass);
    }

    private static CompleteExecutionRequestDto GetCompletionRequest()
    {
        return new CompleteExecutionRequestDto()
        {
            ExecutionId = 1,
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
