using Teczter.Domain.Entities;

namespace Teczter.Adapters.AdapterInterfaces;

public interface ITestRoundAdapter
{
    Task CreateNewTestRound(ExecutionGroupEntity testRound);
    Task<List<ExecutionGroupEntity>> GetAllTestRounds();
    Task<ExecutionGroupEntity?> GetTestRoundById(int id);
    Task<ExecutionGroupEntity?> GetTestRoundByTestRoundName(string testRoundName);
}
