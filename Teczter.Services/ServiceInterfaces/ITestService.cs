using Teczter.Domain.Entities;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestService
{
    Task<TestEntity> CreateNewTest(TestEntity test);
    Task<List<TestEntity>> GetTestSearchResults(string? testName, string? pillarOwner);
    Task<TestEntity?> GetTestById(Guid id);
    Task DeleteTest(TestEntity test);
    Task<TestEntity> UpdateTest(TestEntity currentTest, TestEntity update);
}
