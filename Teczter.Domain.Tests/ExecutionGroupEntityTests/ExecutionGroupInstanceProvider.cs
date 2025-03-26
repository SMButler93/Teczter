using Teczter.Domain.Entities;
using Teczter.Domain.Tests.ExecutionEntityTests;

namespace Teczter.Domain.Tests.ExecutionGroupEntityTests;

public static class ExecutionGroupInstanceFactory
{
    public static ExecutionGroupEntity GetBasicExecutionGroupInstanceWithNumberOfExecutions(int numberOfExecutions)
    {
        return new ExecutionGroupEntity
        {
            CreatedById = 1,
            RevisedById = 1,
            IsDeleted = false,
            ExecutionGroupName = "Basic Instance",
            SoftwareVersionNumber = "1.1.1",
            Executions = ExecutionInstanceProvider.GetMultipleBasicExecutionInstances(numberOfExecutions)
        };
    }
}
