using Teczter.Domain.Entities;

namespace Teczter.Services.AdapterInterfaces;

public interface IExecutionGroupAdapter
{
    IQueryable<ExecutionGroupEntity> GetBasicExecutionGroupSearchQuery();
    Task<ExecutionGroupEntity?> GetExecutionGroupById(int id);
    Task CreateNewExecutionGroup(ExecutionGroupEntity executionGroup);
}
