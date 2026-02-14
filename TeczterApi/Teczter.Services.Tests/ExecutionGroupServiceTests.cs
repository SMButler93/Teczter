using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Services.AdapterInterfaces;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.Tests;

[TestFixture]
public class ExecutionGroupServiceTests
{
    private readonly Mock<IExecutionGroupAdapter> _executionGroupAdapterMock = new();
    private readonly Mock<IExecutionGroupComposer> _executionGroupComposerMock = new();
    private readonly Mock<IValidator<ExecutionGroupEntity>> _executionGroupValidatorMock = new();
    private readonly UnitOfWorkFake _uow = new();

    private ExecutionGroupService _sut = null!;

    [SetUp]
    public void Setup()
    {
        _sut = new(
            _executionGroupAdapterMock.Object,
            _executionGroupComposerMock.Object,
            _executionGroupValidatorMock.Object,
            _uow);
    }

    [Test]
    public async Task CloneExecutionGroup_WhenCalled_ShouldProvideANewUntestedVersionOfTheSpecifiedExecutionGroup()
    {
        //Arrange:
        var executionGroupToClone = GetSingleBasicExecutionGroupInstance();
        const string newName = "Group 2";
        const string newSoftwareVersionNumber = "1.1.2";
        var validationResult = new ValidationResult();
        var ct = new CancellationTokenSource().Token;
        _executionGroupValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<ExecutionGroupEntity>(), It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        //Act:
        var result = await _sut.CloneExecutionGroup(executionGroupToClone, newName, newSoftwareVersionNumber, ct);

        //Assert:
        result.IsValid.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.ExecutionGroupName.ShouldBe(newName);
        result.Value.SoftwareVersionNumber.ShouldBe(newSoftwareVersionNumber);
        result.Value.Executions.First().TestId.ShouldBe(executionGroupToClone.Executions.First().TestId);
        result.Value.Executions.Last().TestId.ShouldBe(executionGroupToClone.Executions.Last().TestId);
    }

    [Test]
    public async Task DeleteExecutionGroup_WhenCalled_ShouldMarkTheExecutionGroupAndAllOwnedTestsAsDeleted()
    {
        //Arrange:
        var executionGroup = GetSingleBasicExecutionGroupInstance();
        var ct = new CancellationTokenSource().Token;

        //Act:
        var result = await _sut.DeleteExecutionGroup(executionGroup, ct);

        //Assert:
        result.IsValid.ShouldBeTrue();
        executionGroup.IsDeleted.ShouldBeTrue();
        executionGroup.Executions.ShouldAllBe(x => x.IsDeleted);
    }

    [Test]
    public async Task DeleteExecutionGroup_WhenGroupIsCompleted_ShouldThrowExeception()
    {
        //Arrange:
        var executionGroup = GetSingleBasicExecutionGroupInstance();
        var ct = new CancellationTokenSource().Token;
        executionGroup.CloseTestRound();
        
        //Act:
        var result = await _sut.DeleteExecutionGroup(executionGroup, ct);

        //Assert:
        result.IsValid.ShouldBeFalse();
        result.ErrorMessages.ShouldNotBeEmpty();
    }

    private static ExecutionGroupEntity GetSingleBasicExecutionGroupInstance()
    {
        return new ExecutionGroupEntity()
        {
            Id = 1,
            CreatedById = 1,
            RevisedById = 1,
            ExecutionGroupName = "Group 1",
            SoftwareVersionNumber = "1.1.1",
            Executions = GetMultipleBasicExecutionInstances()
        };
    }

    private static List<ExecutionEntity> GetMultipleBasicExecutionInstances()
    {
        return
        [
            new ExecutionEntity()
            {
                Id = 1,
                CreatedById = 1,
                RevisedById = 1,
                TestId = 1,
                ExecutionGroup = new()
            },
            new ExecutionEntity()
            {
                Id = 2,
                CreatedById = 1,
                RevisedById = 1,
                TestId = 2,
                ExecutionGroup = new()
            }
        ];
    }
}
