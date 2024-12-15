using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain.Entities;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class TestRoundService(ITestRoundAdapter testRoundAdapter) : ITestRoundService
{
    private readonly ITestRoundAdapter _testRoundAdpater = testRoundAdapter;

    public Task CreateNewTestRound(TestRoundEntity testRound)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTestRound(TestRoundEntity testRound)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TestRoundEntity>> GetAllTestRounds()
    {
        return await _testRoundAdpater.GetAllTestRounds();
    }

    public Task<TestEntity?> GetTestByName(string testRoundName)
    {
        throw new NotImplementedException();
    }

    public Task<TestRoundEntity?> GetTestRoundById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<TestRoundEntity?> UpdateTest(TestRoundEntity testRound)
    {
        throw new NotImplementedException();
    }
}