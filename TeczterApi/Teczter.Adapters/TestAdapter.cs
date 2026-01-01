using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Services.AdapterInterfaces;

namespace Teczter.Adapters;

public class TestAdapter(TeczterDbContext dbContext) : ITestAdapter
{
    public async Task AddNewTest(TestEntity test, CancellationToken ct)
    {
        await dbContext.Tests.AddAsync(test, ct);
    }

    public async Task<TestEntity?> GetTestById(int id, CancellationToken ct)
    {
        return await dbContext.Tests
            .Include(x => x.TestSteps.OrderBy(y => y.StepPlacement))
            .SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public IQueryable<TestEntity> GetTestSearchBaseQuery()
     {
        return dbContext.Tests
            .AsNoTracking()
            .Include(x => x.TestSteps)
            .Where(x => !x.IsDeleted);
    }
}