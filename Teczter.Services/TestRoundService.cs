using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain.Entities;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class TestRoundService(ITestRoundAdapter testRoundAdapter) : ITestRoundService
{
    private readonly ITestRoundAdapter _testRoundAdpater = testRoundAdapter;

    public Task CreateNewTestRound(ExecutionGroupEntity testRound)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTestRound(ExecutionGroupEntity testRound)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ExecutionGroupEntity>> GetAllTestRounds()
    {
        return await _testRoundAdpater.GetAllTestRounds();
    }

    public Task<TestEntity?> GetTestByName(string testRoundName)
    {
        throw new NotImplementedException();
    }

    public Task<ExecutionGroupEntity?> GetTestRoundById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ExecutionGroupEntity?> UpdateTest(ExecutionGroupEntity testRound)
    {
        throw new NotImplementedException();
    }
}