using Teczter.Domain.Entities;
using Teczter.Domain.ValidationObjects;
using Teczter.Services.DTOs.Request;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestService
{
    Task<TeczterValidationResult<TestEntity>> CreateNewTest(TestCommandRequestDto test);
    Task<List<TestEntity>> GetTestSearchResults(string? testTitle, string? pillarOwner);
    Task<TestEntity?> GetTestById(Guid id);
    Task DeleteTest(TestEntity test);
    Task<TeczterValidationResult<TestEntity>> UpdateTest(TestEntity test, TestCommandRequestDto testUpdates);
    Task<TeczterValidationResult<TestEntity>> AddLinkUrl(TestEntity test, string url);
    Task<TeczterValidationResult<TestEntity>> RemoveLinkUrl(TestEntity test, string url);
    Task<TeczterValidationResult<TestEntity>> AddTestStep(TestEntity test, TestStepCommandRequestDto testStep);
    Task<TeczterValidationResult<TestEntity>> RemoveTestStep(TestEntity test, Guid testStepId);
}
