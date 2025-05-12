using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;

namespace Teczter.Domain.Tests;

[TestFixture]
public class ExecutionEntityTests
{
    private ExecutionEntity _sut = null!;

    [SetUp]
    public void Setup()
    {
        _sut = new ExecutionEntity
        {
            CreatedById = 1,
            RevisedById = 1,
            IsDeleted = false,
            TestId = 999,
            ExecutionGroup = new()
        };
    }

    [Test]
    public void Pass_WhenPassed_ShouldBeUpdatedToReflectPass()
    {
        //Arrange:
        var currentRevisedOn = _sut.RevisedOn;

        //Act:
        _sut.Pass(1);

        //Assert:
        _sut.TestedById.ShouldBe(1);
        _sut.RevisedOn.ShouldNotBe(currentRevisedOn);
        _sut.ExecutionState.ShouldBe(ExecutionStateType.Pass);
    }

    [Test]
    public void Fail_WhenFailed_ShouldBeUpdatedToReflectFailure()
    {
        //Arrange:
        var currentRevisedOn = _sut.RevisedOn;
        var failureReason = "fail";

        //Act:
        _sut.Fail(1, 1, failureReason);

        //Assert:
        _sut.TestedById.ShouldBe(1);
        _sut.FailedStepId.ShouldBe(1);
        _sut.FailureReason.ShouldBe(failureReason);
        _sut.ExecutionState.ShouldBe(ExecutionStateType.Fail);
        _sut.RevisedOn.ShouldNotBe(currentRevisedOn);
    }

    [Test]
    public void Reset_WhenReset_ShouldResetFieldsUpdatedSinceCreation()
    {
        //Arrange:
        _sut.Fail(1, 1, "Fail");
        _sut.Delete();
        var currentRevisedOn = _sut.RevisedOn;

        //Act:
        _sut.Reset();

        //Assert:
        _sut.RevisedOn.ShouldNotBe(currentRevisedOn);
        _sut.IsDeleted.ShouldBeFalse();
        _sut.FailedStep.ShouldBeNull();
        _sut.FailureReason.ShouldBeNull();
        _sut.TestedById.ShouldBeNull();
        _sut.ExecutionState.ShouldBe(ExecutionStateType.Untested);
    }

    [Test]
    public void Delete_WhenDeleted_ShouldBeUpdatedToReflectDeletion()
    {
        //Arrange:
        var currentRevisedOn = _sut.RevisedOn;

        //Act:
        _sut.Delete();

        //Assert:
        _sut.IsDeleted.ShouldBeTrue();
        _sut.RevisedOn.ShouldNotBe(currentRevisedOn);
    }

    [Test]
    public void CloneExecution_WhenCloned_ShouldHaveSameValuesExecptForAuditableValues()
    {
        //Arrange & Act:
        var clonedExecution = _sut.CloneExecution();

        //Assert:
        clonedExecution.TestId.ShouldBe(_sut.TestId);
        clonedExecution.Test.ShouldBe(_sut.Test);
        clonedExecution.AssignedUserId.ShouldBe(_sut.AssignedUserId);
        clonedExecution.Notes.ShouldBe(_sut.Notes);
        clonedExecution.ExecutionState.ShouldBe(ExecutionStateType.Untested);
        clonedExecution.CreatedOn.ShouldNotBe(_sut.CreatedOn);
        clonedExecution.RevisedOn.ShouldNotBe(_sut.RevisedOn);
    }
}
