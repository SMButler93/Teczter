using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;

namespace Teczter.Domain.Tests.ExecutionEntityTests;

[TestFixture]
public class ExecutionEntityTests
{
    public static ExecutionEntity GetSubjectUnderTest()
    {
        return ExecutionInstanceProvider.GetBasicExecutionInstance();
    }

    [Test]
    public void Pass_WhenPassed_ShouldBeUpdatedToReflectPass()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();
        var currentRevisedOn = sut.RevisedOn;

        //Act:
        sut.Pass(1);

        //Assert:
        sut.TestedById.ShouldBe(1);
        sut.RevisedOn.ShouldNotBe(currentRevisedOn);
        sut.ExecutionState.ShouldBe(ExecutionStateType.Pass);
    }

    [Test]
    public void Fail_WhenFailed_ShouldBeUpdatedToReflectFailure()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();
        var currentRevisedOn = sut.RevisedOn;
        var failureReason = "fail";

        //Act:
        sut.Fail(1, 1, failureReason);

        //Assert:
        sut.TestedById.ShouldBe(1);
        sut.FailedStepId.ShouldBe(1);
        sut.FailureReason.ShouldBe(failureReason);
        sut.ExecutionState.ShouldBe(ExecutionStateType.Fail);
        sut.RevisedOn.ShouldNotBe(currentRevisedOn);
    }

    [Test]
    public void Reset_WhenReset_ShouldResetFieldsUpdatedSinceCreation()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();
        sut.Fail(1, 1, "Fail");
        sut.Delete();
        var currentRevisedOn = sut.RevisedOn;

        //Act:
        sut.Reset();

        //Assert:
        sut.RevisedOn.ShouldNotBe(currentRevisedOn);
        sut.IsDeleted.ShouldBeFalse();
        sut.FailedStep.ShouldBeNull();
        sut.FailureReason.ShouldBeNull();
        sut.TestedById.ShouldBeNull();
        sut.ExecutionState.ShouldBe(ExecutionStateType.Untested);
    }

    [Test]
    public void Delete_WhenDeleted_ShouldBeUpdatedToReflectDeletion()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();
        var currentRevisedOn = sut.RevisedOn;

        //Act:
        sut.Delete();

        //Assert:
        sut.IsDeleted.ShouldBeTrue();
        sut.RevisedOn.ShouldNotBe(currentRevisedOn);
    }

    [Test]
    public void CloneExecution_WhenCloned_ShouldHaveSameValuesExecptForAuditableValues()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();

        //Act:
        var clonedExecution = sut.CloneExecution();

        //Assert:
        clonedExecution.TestId.ShouldBe(sut.TestId);
        clonedExecution.Test.ShouldBe(sut.Test);
        clonedExecution.AssignedUserId.ShouldBe(sut.AssignedUserId);
        clonedExecution.Notes.ShouldBe(sut.Notes);
        clonedExecution.ExecutionState.ShouldBe(ExecutionStateType.Untested);
    }
}
