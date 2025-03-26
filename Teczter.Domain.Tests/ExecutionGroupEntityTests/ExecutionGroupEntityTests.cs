using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Domain.Tests.ExecutionEntityTests;
using Teczter.Domain.Tests.TestEntityTests;

namespace Teczter.Domain.Tests.ExecutionGroupEntityTests;

[TestFixture]
public class ExecutionGroupEntityTests
{
    public static ExecutionGroupEntity GetSubjectUnderTest()
    {
        return ExecutionGroupInstanceFactory.GetBasicExecutionGroupInstanceWithNumberOfExecutions(2);
    }

    [Test]
    public void DeleteExecutionGroup_WhenDeleted_ShouldDeleteEveryOwnedExecution()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();

        //Act:
        sut.Delete();

        //Assert:
        sut.IsDeleted.ShouldBeTrue();
        sut.Executions.ShouldAllBe(x => x.IsDeleted);
    }

    [Test]
    public void AddExecution_WhenAdded_ShouldBePresentInCollectionOfExecutions()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();
        var newExecution = ExecutionInstanceProvider.GetBasicExecutionInstance();

        //Act:
        sut.AddExecution(newExecution);

        //Assert:
        sut.Executions.Count.ShouldBe(3);
        sut.Executions.ShouldContain(newExecution);
    }

    [Test]
    public void AddNote_WhenAdded_ShouldBePresentInCollectionOfNotes()
    {
        //Arrange
        var sut = GetSubjectUnderTest();
        var note = "new note";

        //Act:
        sut.AddNote(note);

        //Assert:
        sut.ExecutionGroupNotes.Count.ShouldBe(1);
        sut.ExecutionGroupNotes.ShouldContain(note);

    }

    [Test]
    public void CloneExecutionGroup_WhenCloned_ShouldHaveSameValuesExceptForNameAndAuditableProperties()
    {
        //Arrange:
        var sut = GetSubjectUnderTest();
        sut.Executions[0].Test = TestInstanceProvider.GetBasicTestInstanceWithNumberOfTestSteps(1);

        //Act:
        var clonedGroup = sut.CloneExecutionGroup("new execution group", "2.2.2");

        //Assert:
        clonedGroup.Executions.Count.ShouldBe(sut.Executions.Count);
        clonedGroup.Executions[0].CreatedOn.ShouldNotBe(sut.Executions[0].CreatedOn);
        clonedGroup.ExecutionGroupName.ShouldNotBe(sut.ExecutionGroupName);
        clonedGroup.CreatedOn.ShouldNotBe(sut.CreatedOn);
        clonedGroup.Executions[0].Test.Title.ShouldBe(sut.Executions[0].Test.Title);
        clonedGroup.Executions[0].Test.TestSteps.Count.ShouldBe(sut.Executions[0].Test.TestSteps.Count);
    }
}
