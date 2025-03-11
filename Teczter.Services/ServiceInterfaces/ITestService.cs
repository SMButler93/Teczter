using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Services.DTOs.Request;
using Teczter.Services.RequestDtos.Request;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestService
{
    Task<TeczterValidationResult<TestEntity>> CreateNewTest(CreateTestRequestDto test);
    Task<List<TestEntity>> GetTestSearchResults(string? testTitle, string? owningDepartment);
    Task<TestEntity?> GetTestById(int id);
    Task DeleteTest(TestEntity test);
    Task<TeczterValidationResult<TestEntity>> UpdateTest(TestEntity test, UpdateTestRequestDto testUpdates);
    Task<TeczterValidationResult<TestEntity>> AddLinkUrl(TestEntity test, string url);
    Task<TeczterValidationResult<TestEntity>> RemoveLinkUrl(TestEntity test, string url);
    Task<TeczterValidationResult<TestEntity>> AddTestStep(TestEntity test, TestStepCommandRequestDto testStep);
    Task<TeczterValidationResult<TestEntity>> RemoveTestStep(TestEntity test, int testStepId);
    Task<TeczterValidationResult<TestEntity>> UpdateTestStep(TestEntity test, int testStepId, TestStepCommandRequestDto request);
    Task<TeczterValidationResult<TestEntity>> ValidateTestState(TestEntity test);
}
