using Teczter.Domain.Entities;

namespace Teczter.Domain.Tests.ExecutionEntityTests;

public static class ExecutionInstanceProvider
{
    public static List<ExecutionEntity> GetMultipleBasicExecutionInstances(int numberOfInstances)
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

    public static ExecutionEntity GetBasicExecutionInstance()
    {
        return new ExecutionEntity
        {
            CreatedById = 1,
            RevisedById = 1,
            IsDeleted = false,
            TestId = 999
        };
    }
}
