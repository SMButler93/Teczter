using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Data;
using Teczter.Domain.Entities;

namespace Teczter.Adapters;

public class TestAdapter(TeczterDbContext dbContext) : ITestAdapter
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public async Task CreateNewTest(TestEntity test)
    {
        await _dbContext.Tests.AddAsync(test);
    }

    public async Task<TestEntity?> GetTestById(int id)
    {
        return await _dbContext.Tests
            .Include(x => x.TestSteps.OrderBy(y => y.StepPlacement))
            .ThenInclude(y => y.LinkUrls)
            .SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public IQueryable<TestEntity> GetBasicTestSearchBaseQuery()
    {
        return _dbContext.Tests
            .Include(x => x.TestSteps)
            .Where(x => !x.IsDeleted);
    }
}