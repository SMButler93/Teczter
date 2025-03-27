using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;

namespace Teczter.Domain.Tests;

[TestFixture]
public class ExecutionGroupEntityTests
{
    private ExecutionGroupEntity _sut = null!;

    [SetUp]
    public void Setup()
    {
        _sut = new ExecutionGroupEntity
        {
            CreatedById = 1,
            RevisedById = 1,
            IsDeleted = false,
            ExecutionGroupName = "Basic Instance",
            SoftwareVersionNumber = "1.1.1",
            Executions = GetMultipleBasicExecutionInstances(2)
        };
    }

    [Test]
    public void DeleteExecutionGroup_WhenDeleted_ShouldDeleteEveryOwnedExecution()
    {
        //Act:
        _sut.Delete();

        //Assert:
        _sut.IsDeleted.ShouldBeTrue();
        _sut.Executions.ShouldAllBe(x => x.IsDeleted);
    }

    [Test]
    public void AddExecution_WhenAdded_ShouldBePresentInCollectionOfExecutions()
    {
        //Arrange:
        var newExecution = new ExecutionEntity
        {
            CreatedById = 1,
            RevisedById = 1,
            IsDeleted = false,
            TestId = 999
        };

        //Act:
        _sut.AddExecution(newExecution);

        //Assert:
        _sut.Executions.Count.ShouldBe(3);
        _sut.Executions.ShouldContain(newExecution);
    }

    [Test]
    public void AddNote_WhenAdded_ShouldBePresentInCollectionOfNotes()
    {
        //Arrange
        var note = "new note";

        //Act:
        _sut.AddNote(note);

        //Assert:
        _sut.ExecutionGroupNotes.Count.ShouldBe(1);
        _sut.ExecutionGroupNotes.ShouldContain(note);

    }

    [Test]
    public void CloneExecutionGroup_WhenCloned_ShouldHaveSameValuesExceptForNameAndAuditableProperties()
    {
        //Arrange:
        _sut.Executions[0].Test = GetBasicTestInstance();

        //Act:
        var clonedGroup = _sut.CloneExecutionGroup("new execution group", "2.2.2");

        //Assert:
        clonedGroup.Executions.Count.ShouldBe(_sut.Executions.Count);
        clonedGroup.Executions[0].CreatedOn.ShouldNotBe(_sut.Executions[0].CreatedOn);
        clonedGroup.ExecutionGroupName.ShouldNotBe(_sut.ExecutionGroupName);
        clonedGroup.CreatedOn.ShouldNotBe(_sut.CreatedOn);
        clonedGroup.Executions[0].Test.Title.ShouldBe(_sut.Executions[0].Test.Title);
        clonedGroup.Executions[0].Test.TestSteps.Count.ShouldBe(_sut.Executions[0].Test.TestSteps.Count);
    }

    private static TestEntity GetBasicTestInstance()
    {
        return new TestEntity()
        {
            IsDeleted = false,
            Title = "Basic test instance.",
            Description = "Basic instance for testing.",
            OwningDepartment = Department.Operations,
            TestSteps = []
        };
    }

    private static List<ExecutionEntity> GetMultipleBasicExecutionInstances(int numberOfInstances)
    {
        var executions = new List<ExecutionEntity>();

        for (var i = 1; i <= numberOfInstances; i++)
        {
            var execution = new ExecutionEntity
            {
                CreatedById = 1,
                RevisedById = 1,
                IsDeleted = false,
                TestId = i,
            };

            executions.Add(execution);
        }

        return executions;
    }
}
