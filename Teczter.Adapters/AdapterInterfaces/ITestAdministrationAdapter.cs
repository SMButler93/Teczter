using Teczter.Domain.Entities;

namespace Teczter.Adapters.AdapterInterfaces;

public interface ITestAdministrationAdapter
{
    Task CreateNewTest(TestEntity test);
    Task<List<TestEntity>> GetAllTestsInTestRound(Guid testRoundId);
    Task<List<TestEntity>> GetAllUsersAssignedTests(Guid userid);
    Task<TestEntity?> GetTestById(Guid id);
    Task<TestEntity?> GetTestByName(string testName);
}