using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Domain.Exceptions;
using Teczter.Persistence;
using Teczter.Services.RequestDtos;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class TestService : ITestService
{
    private readonly ITestAdapter _testAdapter;
    private readonly IExecutionAdapter _executionAdapter;
    private readonly ITestBuilder _builder;
    private readonly IUnitOfWork _uow;
    private readonly IValidator<TestEntity> _testValidator;

    public TestService(
        ITestAdapter testAdapter,
        IExecutionAdapter executionAdapter,
        ITestBuilder builder,
        IUnitOfWork uow,
        IValidator<TestEntity> testValidator)
    {
        _testAdapter = testAdapter;
        _executionAdapter = executionAdapter;
        _builder = builder;
        _uow = uow;
        _testValidator = testValidator;
    }

    public async Task<TeczterValidationResult<TestEntity>> AddLinkUrl(TestEntity test, string url)
    {
        _builder.UsingContext(test)
            .AddLinkUrl(url);

        var result = await ValidateTestState(test);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> AddTestStep(TestEntity test, CreateTestStepRequestDto testStep)
    {
        _builder.UsingContext(test)
            .AddStep(testStep);

        var result = await ValidateTestState(test);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> CreateNewTest(CreateTestRequestDto request)
    {
        var test = _builder
            .NewInstance()
            .SetTitle(request.Title)
            .SetDescription(request.Description)
            .SetOwningDepartment(request.OwningDepartment)
            .AddLinkUrls(request.LinkUrls ?? [])
            .AddSteps(request.TestSteps)
            .Build();

        await _testAdapter.CreateNewTest(test);

        var result = await ValidateTestState(test);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task DeleteTest(TestEntity test)
    {
        test.Delete();
        var executionsToDelete = await _executionAdapter.GetExecutionsForTest(test.Id);

        foreach (var execution in executionsToDelete)
        {
            execution.Delete();
        }

        await _uow.CommitChanges();
    }

    public async Task<TestEntity?> GetTestById(int id)
    {
        return await _testAdapter.GetTestById(id);
    }

    public async Task<List<TestEntity>> GetTestSearchResults(string? testTitle, string? owningDepartment)
    {
        var TestSearchQuery = _testAdapter.GetBasicTestSearchBaseQuery();
        
        TestSearchQuery = testTitle == null ? TestSearchQuery : TestSearchQuery.Where(x => x.Title.Contains(testTitle));
        TestSearchQuery = owningDepartment == null ? TestSearchQuery : TestSearchQuery.Where(x => x.OwningDepartment.ToString() == owningDepartment);

        return await TestSearchQuery.ToListAsync();
    }

    public async Task<TeczterValidationResult<TestEntity>> RemoveLinkUrl(TestEntity test, string url)
    {
        var existingUrl = test.Urls.SingleOrDefault(x => x.Equals(x, StringComparison.OrdinalIgnoreCase)) ?? 
            throw new TeczterValidationException("Cannot remove a link that does not belong to this test");

        test.RemoveLinkUrl(existingUrl);

        var result = await ValidateTestState(test);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> RemoveTestStep(TestEntity test, int testStepId)
    {
        var testStep = test.TestSteps.SingleOrDefault(x => x.Id == testStepId && !x.IsDeleted) ?? 
            throw new TeczterValidationException("Cannot remove a test step that does not exist, has already been deleted, " +
            "or does not belong to this test.");

        test.RemoveTestStep(testStep);

        var result = await ValidateTestState(test);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> UpdateTest(TestEntity test, UpdateTestRequestDto testUpdates)
    {
        _builder.UsingContext(test)
            .SetTitle(testUpdates.Title)
            .SetDescription(testUpdates.Description)
            .SetOwningDepartment(testUpdates.OwningDepartment);

        var result = await ValidateTestState(test);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> UpdateTestStep(TestEntity test, int testStepId, UpdateTestStepRequestDto request)
    {
        var testStep = test.TestSteps.SingleOrDefault(x => x.Id == testStepId && !x.IsDeleted) ??
            throw new TeczterValidationException("Cannot update a test step that does not exist or has already been deleted");

        testStep.Update(request.StepPlacement, request.Instructions, request.Urls);

        test.SetCorrectStepPlacementValuesOnUpdate();

        var result = await ValidateTestState(test);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> ValidateTestState(TestEntity test)
    {
        var result = await _testValidator.ValidateAsync(test);

        if (!result.IsValid)
        {
            return TeczterValidationResult<TestEntity>.Fail(result.Errors.Select(x => x.ErrorMessage).ToArray());
        }

        return TeczterValidationResult<TestEntity>.Succeed(test);
    }

    private async Task EvaluateValidationResultAndPersist(TeczterValidationResult<TestEntity> result)
    {
        if (!result.IsValid)
        {
            _uow.Rollback();
            return;
        }

        await _uow.CommitChanges();
    }
}