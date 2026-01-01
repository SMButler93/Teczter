using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos.Tests;
using Teczter.Services.RequestDtos.TestSteps;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestService
{
    Task<TeczterValidationResult<TestEntity>> CreateNewTest(CreateTestRequestDto test, CancellationToken ct);
    Task<List<TestEntity>> GetTestSearchResults(int pageNumber, string? testTitle, string? owningDepartment, CancellationToken ct);
    Task<TestEntity?> GetTestById(int id, CancellationToken ct);
    Task DeleteTest(TestEntity test, CancellationToken ct);
    Task<TeczterValidationResult<TestEntity>> UpdateTest(TestEntity test, UpdateTestRequestDto testUpdates, CancellationToken ct);
    Task<TeczterValidationResult<TestEntity>> AddLinkUrl(TestEntity test, string url, CancellationToken ct);
    Task<TeczterValidationResult<TestEntity>> RemoveLinkUrl(TestEntity test, string url, CancellationToken ct);
    Task<TeczterValidationResult<TestEntity>> AddTestStep(TestEntity test, CreateTestStepRequestDto testStep, CancellationToken ct);
    Task<TeczterValidationResult<TestEntity>> RemoveTestStep(TestEntity test, int testStepId, CancellationToken ct);
    Task<TeczterValidationResult<TestEntity>> UpdateTestStep(TestEntity test, int testStepId, UpdateTestStepRequestDto request, CancellationToken ct);
    Task<TeczterValidationResult<TestEntity>> ValidateTestState(TestEntity test, CancellationToken ct);
}
