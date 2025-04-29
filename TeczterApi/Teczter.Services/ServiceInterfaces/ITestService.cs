using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos.Tests;
using Teczter.Services.RequestDtos.TestSteps;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestService
{
    Task<TeczterValidationResult<TestEntity>> CreateNewTest(CreateTestRequestDto test);
    Task<List<TestEntity>> GetTestSearchResults(int pageNumber, string? testTitle, string? owningDepartment);
    Task<TestEntity?> GetTestById(int id);
    Task DeleteTest(TestEntity test);
    Task<TeczterValidationResult<TestEntity>> UpdateTest(TestEntity test, UpdateTestRequestDto testUpdates);
    Task<TeczterValidationResult<TestEntity>> AddLinkUrl(TestEntity test, string url);
    Task<TeczterValidationResult<TestEntity>> RemoveLinkUrl(TestEntity test, string url);
    Task<TeczterValidationResult<TestEntity>> AddTestStep(TestEntity test, CreateTestStepRequestDto testStep);
    Task<TeczterValidationResult<TestEntity>> RemoveTestStep(TestEntity test, int testStepId);
    Task<TeczterValidationResult<TestEntity>> UpdateTestStep(TestEntity test, int testStepId, UpdateTestStepRequestDto request);
    Task<TeczterValidationResult<TestEntity>> ValidateTestState(TestEntity test);
}
