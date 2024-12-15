using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Data;
using Teczter.Domain.Entities;

namespace Teczter.Adapters;

public class TestRoundAdapter(TeczterDbContext dbContext) : ITestRoundAdapter
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public Task CreateNewTestRound(TestRoundEntity testRound)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TestRoundEntity>> GetAllTestRounds()
    {
        return await _dbContext.TestRounds.ToListAsync();
    }

    public Task<TestRoundEntity?> GetTestRoundById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<TestRoundEntity?> GetTestRoundByTestRoundName(string testRoundName)
    {
        return await _dbContext.TestRounds.Where(x => x.TestRoundName == testRoundName).SingleOrDefaultAsync();
    }
}
