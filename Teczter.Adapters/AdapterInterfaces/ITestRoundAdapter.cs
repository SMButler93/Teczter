using Teczter.Domain.Entities;

namespace Teczter.Adapters.AdapterInterfaces;

public interface ITestRoundAdapter
{
    Task CreateNewTestRound(TestRoundEntity testRound);
    Task<List<TestRoundEntity>> GetAllTestRounds();
    Task<TestRoundEntity?> GetTestRoundById(int id);
    Task<TestRoundEntity?> GetTestRoundByTestRoundName(string testRoundName);
}
