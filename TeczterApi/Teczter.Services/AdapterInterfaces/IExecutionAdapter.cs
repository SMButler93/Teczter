using Teczter.Domain.Entities;

namespace Teczter.Services.AdapterInterfaces;

public interface IExecutionAdapter
{
    Task<List<ExecutionEntity>> GetExecutionsForTest(int testId, CancellationToken ct);
    Task<ExecutionEntity?> GetExecutionById(int id, CancellationToken ct);
}
