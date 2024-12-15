using Teczter.Domain.Entities;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestAdministrationService
{
    Task CreateNewTest(TestEntity test);
    Task<List<TestEntity>> GetTestsInTestRound(string testRoundName);
    Task<List<TestEntity>> GetAllUsersAssignedTests(Guid userId);
    Task<TestEntity?> GetTestById(Guid id);
    Task<TestEntity?> GetTestByName(string testRoundName);
    Task DeleteTest(TestEntity test);
    Task<TestEntity?> UpdateTest(TestEntity test);
}
