using NUnit.Framework;
using Shouldly;
using Teczter.Domain.Entities;
using Teczter.Services.Composers;
using Teczter.Services.RequestDtos.Executions;

namespace Teczter.Services.Tests;

[TestFixture]
public class ExecutionGroupComposerTests
{
    private ExecutionGroupComposer _sut = null!;

    [SetUp]
    public void Setup()
    {
        _sut = new();
        _sut.UsingContext(GetBasicSingleExecutionGroupInstance());
    }
    [Test]
    public void AddExecution_WhenAdded_ShouldExistInTheProvidedInstance()
    {
        //Arrange:
        var execution = GetSpecifiedNumberOfBasicExecutionInstances(1).Single();

        //Act:
        var result = _sut.AddExecution(execution).Build();

        //Assert:
        result.Executions.Single().ShouldBe(execution);
    }

    [Test]
    public void AddExecutions_WhenMultipleExecutionsAdded_ShouldAllExistInInstanceProvided()
    {
        //Arrange:
        var executions = GetSpecifiedNumberOfBasicExecutionInstances(2);

        //Act:
        var result = _sut.AddExecutions(executions).Build();

        //Assert:
        result.Executions.ShouldBe(executions);
    }

    [Test]
    public void AddExecution_WhenAddedFromRequestDto_ShouldExistInTheInstanceProvided()
    {
        //Arrange:
        var execution = GetSpecifiedNumberOfBasicCreateExecutionRequestDtos(1).Single();

        //Act:
        var result = _sut.AddExecution(execution).Build();

        //Assert:
        result.Executions.Single().TestId.ShouldBe(execution.TestId);
    }

    [Test]
    public void AddExecutions_WhenAddedFromRequestDtos_ShouldExistInProvidedInstance()
    {
        //Arrange:
        var executions = GetSpecifiedNumberOfBasicCreateExecutionRequestDtos(2);

        //Act:
        var result = _sut.AddExecutions(executions).Build();

        //Assert:
        result.Executions.Count.ShouldBe(executions.Count);
        result.Executions.First().TestId.ShouldBe(executions.First().TestId);
        result.Executions.Last().TestId.ShouldBe(executions.Last().TestId);
    }

    [Test]
    public void SetExecutionGroupNotes_WhenReceivesNull_ShouldSetToEmptyCollection()
    {
        //Arrange & Act:
        var result = _sut.SetExecutionGroupNotes(null).Build();

        //Assert:
        result.ExecutionGroupNotes.ShouldNotBeNull();
        result.ExecutionGroupNotes.Count.ShouldBe(0);
    }

    [Test]
    public void SetExecutionGroupNotes_WhenReceivesValues_ShouldExistInProvidedInstance()
    {
        //Arrange:
        List<string> notes = ["Note 1", "Note 2"];

        //Act:
        var result = _sut.SetExecutionGroupNotes(notes).Build();

        //Assert:
        result.ExecutionGroupNotes.ShouldBe(notes);
    }

    [Test]
    public void SetName_WhenCalled_ShouldSetNameOnProvidedInstance()
    {
        //Arrange:
        var name = "group 1";

        //Act:
        var result = _sut.SetName(name).Build();

        //Assert:
        result.ExecutionGroupName.ShouldBe(name);
    }

    [Test]
    public void SetSoftwareVersionNumber_WhenCalled_ShouldSetSoftwareVersionNumberOnProvidedInstance()
    {
        //Arrange:
        var versionNumber = "1.1.1";

        //Act:
        var result = _sut.SetSoftwareVersionNumber(versionNumber).Build();

        //Assert:
        result.SoftwareVersionNumber.ShouldBe(versionNumber);
    }

    private static ExecutionGroupEntity GetBasicSingleExecutionGroupInstance()
    {
        return new ExecutionGroupEntity()
        {
            Id = 1,
            CreatedById = 1,
            RevisedById = 1,
            ExecutionGroupName = "Execution Group 1",
            SoftwareVersionNumber = "1.1.1"
        };
    }

    private static List<ExecutionEntity> GetSpecifiedNumberOfBasicExecutionInstances(int numberOfInstances)
    {
        List<ExecutionEntity> executions = [];

        for (int i = 0; i < numberOfInstances; i++)
        {
            var execution = new ExecutionEntity()
            {
                Id = i,
                CreatedById =i,
                RevisedById = i,
                ExecutionGroupId = i,
                TestId = i
            };

            executions.Add(execution);
        }

        return executions;
    }

    private static List<CreateExecutionRequestDto> GetSpecifiedNumberOfBasicCreateExecutionRequestDtos(int numberOfInstances)
    {
        List<CreateExecutionRequestDto> executions = [];

        for (int i = 0; i < numberOfInstances; i++)
        {
            var execution = new CreateExecutionRequestDto()
            {
                TestId = i
            };

            executions.Add(execution);
        }

        return executions;
    }
}
