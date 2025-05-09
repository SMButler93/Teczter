using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Domain.Exceptions;
using Teczter.Services.AdapterInterfaces;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.Tests;

[TestFixture]
public class ExecutionGroupServiceTests
{
    private Mock<IExecutionGroupAdapter> _executionGroupAdapterMock = new();
    private Mock<IExecutionGroupComposer> _executionGroupComposerMock = new();
    private Mock<IValidator<ExecutionGroupEntity>> _executionGroupValidatorMock = new();
    private UnitOfWorkFake _uow = new();

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
        var newName = "Group 2";
        var newSoftwareVersionNumber = "1.1.2";
        var validationResult = new ValidationResult();
        _executionGroupValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<ExecutionGroupEntity>(), It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        //Act:
        var result = await _sut.CloneExecutionGroup(executionGroupToClone, newName, newSoftwareVersionNumber);

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

        //Act:
        await _sut.DeleteExecutionGroup(executionGroup);

        //Assert:
        executionGroup.IsDeleted.ShouldBeTrue();
        executionGroup.Executions.ShouldAllBe(x => x.IsDeleted);
    }

    [Test]
    public async Task DeleteExecutionGroup_WhenGroupIsCompleted_ShouldThrowExeception()
    {
        //Arrange:
        var executionGroup = GetSingleBasicExecutionGroupInstance();
        executionGroup.CloseTestRound();

        //Act & Assert:
        await Should.ThrowAsync<TeczterValidationException>(() => _sut.DeleteExecutionGroup(executionGroup));
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
                TestId = 1
            },
            new ExecutionEntity()
            {
                Id = 2,
                CreatedById = 1,
                RevisedById = 1,
                TestId = 2
            }
        ];
    }
}
