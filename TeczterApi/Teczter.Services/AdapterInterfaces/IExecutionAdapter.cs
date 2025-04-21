using Teczter.Domain.Entities;

namespace Teczter.Services.AdapterInterfaces;

public interface IExecutionAdapter
{
    Task<List<ExecutionEntity>> GetExecutionsForTest(int testId);
    Task<ExecutionEntity?> GetExecutionById(int id);
}
