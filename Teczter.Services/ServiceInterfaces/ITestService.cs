using Teczter.Domain.Entities;
using Teczter.Services.DTOs.Request;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestService
{
    Task<TestEntity> CreateNewTest(TestCommandRequestDto test);
    Task<List<TestEntity>> GetTestSearchResults(string? testTitle, string? pillarOwner);
    Task<TestEntity?> GetTestById(Guid id);
    Task DeleteTest(TestEntity test);
    Task UpdateTest(TestEntity test, TestCommandRequestDto testUpdates);
    Task AddLinkUrl(TestEntity test, string url);
    Task RemoveLinkUrl(TestEntity test, string url);
    Task AddTestStep(TestEntity test, TestStepCommandRequestDto testStep);
    Task RemoveTestStep(TestEntity test, Guid testStepId);
}
