using Teczter.Domain.Entities;
using Teczter.Services.AdapterInterfaces;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class ExecutionService : IExecutionService
{
    private readonly IExecutionAdapter _executionAdapter;

    public ExecutionService(IExecutionAdapter executionAdapter)
    {
        _executionAdapter = executionAdapter;
    }

    public async Task<ExecutionEntity?> GetExecutionById(int id)
    {
        return await _executionAdapter.GetExecutionById(id);
    }
}
