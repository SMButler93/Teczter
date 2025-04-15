using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Services.ValidationRepositoryInterfaces;

namespace Teczter.Adapters.ValidationRepositories;

public class TestValidationRespository(TeczterDbContext dbContext) : ITestValidationRepository
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public List<TestEntity> GetTestEntitiesWithTitle(string title)
    {
        return _dbContext.Tests.Where(x => x.Title.ToLower() == title.ToLower() && !x.IsDeleted).ToList();
    }
}
