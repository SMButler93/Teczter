using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Data;
using Teczter.Domain.Entities;

namespace Teczter.Adapters;

public class TestAdapter(TeczterDbContext dbContext) : ITestAdapter
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public Task CreateNewTest(TestEntity test)
    {
        throw new NotImplementedException();
    }

    public IQueryable<TestEntity> GetTestSearchBaseQuery()
    {
        return _dbContext.Tests.Include(x => x.TestSteps);
    }
}
