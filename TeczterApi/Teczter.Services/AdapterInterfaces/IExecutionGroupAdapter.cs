using Teczter.Domain.Entities;

namespace Teczter.Services.AdapterInterfaces;

public interface IExecutionGroupAdapter
{
    IQueryable<ExecutionGroupEntity> GetBasicExecutionGroupSearchQuery();
    Task<ExecutionGroupEntity?> GetExecutionGroupById(int id, CancellationToken ct);
    Task AddNewExecutionGroup(ExecutionGroupEntity executionGroup, CancellationToken ct);
}
