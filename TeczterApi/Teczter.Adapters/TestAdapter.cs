using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Services.AdapterInterfaces;

namespace Teczter.Adapters;

public class TestAdapter(TeczterDbContext _dbContext) : ITestAdapter
{
    public async Task AddNewTest(TestEntity test)
    {
        await _dbContext.Tests.AddAsync(test);
    }

    public async Task<TestEntity?> GetTestById(int id)
    {
        return await _dbContext.Tests
            .Include(x => x.TestSteps.OrderBy(y => y.StepPlacement))
            .SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public IQueryable<TestEntity> GetTestSearchBaseQuery()
     {
        return _dbContext.Tests
            .AsNoTracking()
            .Include(x => x.TestSteps)
            .Where(x => !x.IsDeleted);
    }
}