using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Data;
using Teczter.Domain.Entities;

namespace Teczter.Adapters;

public class TestRoundAdapter(TeczterDbContext dbContext) : ITestRoundAdapter
{
    private readonly TeczterDbContext _dbContext = dbContext;

    public Task CreateNewTestRound(ExecutionGroupEntity testRound)
    {
        throw new NotImplementedException();
    }

    public Task<List<ExecutionGroupEntity>> GetAllTestRounds()
    {
        throw new NotImplementedException();
    }

    public Task<ExecutionGroupEntity?> GetTestRoundById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ExecutionGroupEntity?> GetTestRoundByTestRoundName(string testRoundName)
    {
        throw new NotImplementedException();
    }
}
