using Teczter.Domain.Entities;

namespace Teczter.Services.ServiceInterfaces;

public interface IExecutionService
{
    Task<ExecutionEntity?> GetExecutionById(int id);
}
