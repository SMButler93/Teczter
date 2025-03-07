using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Domain.Entities;

namespace Teczter.Adapters.ValidationRepositories.TestValidationRespositories;

public class TestValidationRespository(TeczterDbContext dbContext) : ITestValidationRepository
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public List<TestEntity> GetTestEntitiesWithTitle(string title)
    {
        return _dbContext.Tests.Where(x => x.Title.ToLower() == title.ToLower() && !x.IsDeleted).ToList();
    }
}
