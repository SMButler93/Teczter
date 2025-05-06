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
        result.Executions.Single().CreatedById.ShouldBe(execution.CreatedById);
        result.Executions.Single().RevisedById.ShouldBe(execution.RevisedById);
        result.Executions.Single().Id.ShouldBe(execution.ExecutionGroupId);
        result.Executions.Single().TestId.ShouldBe(execution.TestId);
    }

    [Test]
    public void AddExecutions_WhenMultipleExecutionsAdded_ShouldAllExistInInstanceProvided()
    {

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
