using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Services.ValidationRepositoryInterfaces;

namespace Teczter.Adapters.ValidationRepositories;

public class ExecutionGroupValidationRepository(TeczterDbContext dbContext) : IExecutionGroupValidationRepository
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public List<ExecutionGroupEntity> GetExecutionGroupsWithName(string name)
    {
        return _dbContext.ExecutionGroups.Where(x => x.ExecutionGroupName.ToLower() == name.ToLower()).ToList();
    }

    public List<ExecutionGroupEntity> GetExecutionGroupsWithSoftwareVersionNumber(string versionNumber)
    {
        return _dbContext.ExecutionGroups.Where(x => x.SoftwareVersionNumber == versionNumber).ToList();
    }
}
