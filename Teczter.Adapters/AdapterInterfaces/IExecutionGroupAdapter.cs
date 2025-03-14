using Teczter.Domain.Entities;

namespace Teczter.Adapters.AdapterInterfaces;

public interface IExecutionGroupAdapter
{
    IQueryable<ExecutionGroupEntity> GetBasicExecutionGroupSearchQuery();
    Task<ExecutionGroupEntity?> GetExecutionGroupById(int id);
}
