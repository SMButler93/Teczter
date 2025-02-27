using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Domain.Entities;

namespace Teczter.Adapters.ValidationRepositories.TestValidationRespositories;

public class TestValidationRespository(TeczterDbContext dbContext) : ITestValidationRepository
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public async Task<List<TestEntity>> GetTestEntitiesWithTitle(string title)
    {
        return await _dbContext.Tests.Where(x => x.Title.ToLower() == title.ToLower()).ToListAsync();
    }
}
