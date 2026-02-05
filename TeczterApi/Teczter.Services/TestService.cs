using FluentValidation;
using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Infrastructure.Cache;
using Teczter.Persistence;
using Teczter.Services.AdapterInterfaces;
using Teczter.Services.RequestDtos.Tests;
using Teczter.Services.RequestDtos.TestSteps;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class TestService(ITestAdapter _testAdapter,
        IExecutionAdapter _executionAdapter,
        ITestComposer _composer,
        IUnitOfWork _uow,
        IValidator<TestEntity> _testValidator,
        ITeczterCache<TestEntity> _cache) : ITestService
{
    public async Task<TeczterValidationResult<TestEntity>> AddLinkUrl(TestEntity test, string url, CancellationToken ct)
    {
        _composer.UsingContext(test)
            .AddLinkUrl(url)
            .Build();
        
        var result = await ValidateTestState(test, ct);

        if (result.IsValid)
        {
            await Task.WhenAll(_uow.SaveChanges(ct), _cache.RemoveCache(test.Id, ct));
        }
        
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> AddTestStep(TestEntity test, CreateTestStepRequestDto testStep, CancellationToken ct)
    {
        _composer.UsingContext(test)
            .AddStep(testStep)
            .Build();
        
        var result = await ValidateTestState(test, ct);

        if (result.IsValid)
        {
            await Task.WhenAll(_uow.SaveChanges(ct), _cache.RemoveCache(test.Id, ct));
        }
        
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> CreateNewTest(CreateTestRequestDto request, CancellationToken ct)
    {
        //Set created by user ID when users implemented.
        var test = _composer
            .SetTitle(request.Title)
            .SetDescription(request.Description)
            .SetOwningDepartment(request.OwningDepartment)
            .AddLinkUrls(request.LinkUrls ?? [])
            .AddSteps(request.TestSteps)
            .Build();
        
        var result = await ValidateTestState(test, ct);

        if (result.IsValid)
        {
            await Task.WhenAll(_uow.SaveChanges(ct), _cache.RemoveCache(test.Id, ct));
        }
        
        return result;
    }

    public async Task DeleteTest(TestEntity test, CancellationToken ct)
    {
        test.Delete();

        var executions = await _executionAdapter.GetExecutionsForTest(test.Id, ct);
        var executionsToDelete = executions.Where(x => x.ExecutionState is ExecutionStateType.Untested);

        foreach (var execution in executionsToDelete)
        {
            execution.Delete();
        }

        await Task.WhenAll(_uow.SaveChanges(ct), _cache.RemoveCache(test.Id, ct));
    }

    public async Task<TestEntity?> GetTestById(int id, CancellationToken ct)
    {
        var test = await _cache.GetCachedResult(id, ct) ?? await _testAdapter.GetTestById(id, ct);

        if (test is not null)
        {
            await _cache.SetCache(test, ct);
        }

        return test;
    }

    public async Task<List<TestEntity>> GetTestSearchResults(int pageNumber, string? testTitle, string? owningDepartment, CancellationToken ct)
    {
        return await _testAdapter.GetTestSearchResults(pageNumber, testTitle, owningDepartment, ct);
    }

    public async Task<TeczterValidationResult<TestEntity>> RemoveLinkUrl(TestEntity test, string url, CancellationToken ct)
    { 
        test.RemoveLinkUrl(url);
        
        var result = await ValidateTestState(test, ct);

        if (result.IsValid)
        {
            await Task.WhenAll(_uow.SaveChanges(ct), _cache.RemoveCache(test.Id, ct));
        }
        
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> RemoveTestStep(TestEntity test, int testStepId, CancellationToken ct)
    {
        test.RemoveTestStep(testStepId);
        
        var result = await ValidateTestState(test, ct);

        if (result.IsValid)
        {
            await Task.WhenAll(_uow.SaveChanges(ct), _cache.RemoveCache(test.Id, ct));
        }
        
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> UpdateTest(TestEntity test, UpdateTestRequestDto testUpdates, CancellationToken ct)
    {
        _composer.UsingContext(test)
            .SetTitle(testUpdates.Title)
            .SetDescription(testUpdates.Description)
            .SetOwningDepartment(testUpdates.OwningDepartment)
            .Build();
        
        var result = await ValidateTestState(test, ct);

        if (result.IsValid)
        {
            await Task.WhenAll(_uow.SaveChanges(ct), _cache.RemoveCache(test.Id, ct));
        }
        
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> UpdateTestStep(TestEntity test, int testStepId, UpdateTestStepRequestDto request, CancellationToken ct)
    {
        var testStep = test.TestSteps.Single(x => x.Id == testStepId && !x.IsDeleted);

        testStep.Update(request.StepPlacement, request.Instructions, request.Urls);

        test.SetCorrectStepPlacementValuesOnUpdate();

        var result = await ValidateTestState(test, ct);

        if (result.IsValid)
        {
            await Task.WhenAll(_uow.SaveChanges(ct), _cache.RemoveCache(test.Id, ct));
        }
        
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> ValidateTestState(TestEntity test, CancellationToken ct)
    {
        var result = await _testValidator.ValidateAsync(test, ct);

        return result.IsValid
            ? TeczterValidationResult<TestEntity>.Succeed(test)
            : TeczterValidationResult<TestEntity>.Fail(result.Errors.Select(x => x.ErrorMessage).ToArray());
    }
}