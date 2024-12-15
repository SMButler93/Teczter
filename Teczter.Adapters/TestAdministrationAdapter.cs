using Microsoft.EntityFrameworkCore;
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

    public async Task<List<TestEntity>> GetAllTestsInTestRound(Guid testRoundId)
    {
        return await _dbContext.Tests.Where(x => x.TestingRoundId == testRoundId).ToListAsync();
    }

    public async Task<List<TestEntity>> GetAllUsersAssignedTests(Guid userId)
    {
        return await _dbContext.Tests.Where(x => x.AssignedUserId == userId).ToListAsync();
    }

    public Task<TestEntity?> GetTestById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<TestEntity?> GetTestByName(string testName)
    {
        throw new NotImplementedException();
    }
}
