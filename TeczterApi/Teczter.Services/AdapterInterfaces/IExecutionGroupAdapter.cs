using Teczter.Domain.Entities;

namespace Teczter.Services.AdapterInterfaces;

public interface IExecutionGroupAdapter
{
    Task<List<ExecutionGroupEntity>> GetExecutionGroupSearchResults(int pageNumber, string? executionGroupName, string? versionNumber, CancellationToken ct);
    Task<ExecutionGroupEntity?> GetExecutionGroupById(int id, CancellationToken ct);
    Task AddNewExecutionGroup(ExecutionGroupEntity executionGroup, CancellationToken ct);
}
