using Teczter.Domain.Entities;

namespace Teczter.Adapters.AdapterInterfaces;

public interface IExecutionAdapter
{
    Task<List<ExecutionEntity>> GetExecutionsForTest(int testId);
}
