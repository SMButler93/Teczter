using Teczter.Adapters.AdapterInterfaces;
using Teczter.Data;
using Teczter.Domain.Entities;

namespace Teczter.Adapters;

public class TestAdministrationAdapter(TeczterDbContext dbContext) : ITestAdministrationAdapter
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public Task CreateNewTest(TestEntity test)
    {
        throw new NotImplementedException();
    }

    public IQueryable<TestEntity> GetTestSearchBaseQuery()
    {
        return _dbContext.Tests;
    }
}
