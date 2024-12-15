using Teczter.Domain.Entities;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestRoundService
{
    Task CreateNewTestRound(TestRoundEntity testRound);
    Task<List<TestRoundEntity>> GetAllTestRounds();
    Task<TestRoundEntity?> GetTestRoundById(int id);
    Task<TestEntity?> GetTestByName(string testRoundName);
    Task DeleteTestRound(TestRoundEntity testRound);
    Task<TestRoundEntity?> UpdateTest(TestRoundEntity testRound);
}
