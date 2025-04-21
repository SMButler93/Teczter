using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Services.ValidationRepositoryInterfaces;

namespace Teczter.Adapters.ValidationRepositories;

public class TestValidationRepository(TeczterDbContext dbContext) : ITestValidationRepository
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public List<TestEntity> GetTestEntitiesWithTitle(string title)
    {
        return _dbContext.Tests
            .Where(x => x.Title == title && !x.IsDeleted)
            .AsNoTracking()
            .ToList();
    }
}
