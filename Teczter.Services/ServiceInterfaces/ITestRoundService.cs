using Teczter.Domain.Entities;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestRoundService
{
    Task CreateNewTestRound(ExecutionGroupEntity testRound);
    Task<List<ExecutionGroupEntity>> GetAllTestRounds();
    Task<ExecutionGroupEntity?> GetTestRoundById(int id);
    Task<TestEntity?> GetTestByName(string testRoundName);
    Task DeleteTestRound(ExecutionGroupEntity testRound);
    Task<ExecutionGroupEntity?> UpdateTest(ExecutionGroupEntity testRound);
}
