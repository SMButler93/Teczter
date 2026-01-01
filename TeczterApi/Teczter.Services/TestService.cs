using FluentValidation;
using Microsoft.EntityFrameworkCore;
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

public class TestService(ITestAdapter testAdapter,
        IExecutionAdapter executionAdapter,
        ITestComposer composer,
        IUnitOfWork uow,
        IValidator<TestEntity> testValidator,
        ITeczterCache<TestEntity> cache) : ITestService
{
    private const int TestsPerPage = 15;

    public async Task<TeczterValidationResult<TestEntity>> AddLinkUrl(TestEntity test, string url, CancellationToken ct)
    {
        var testResult = composer.UsingContext(test)
            .AddLinkUrl(url)
            .Build();

        if (!testResult.IsValid)
        {
            return testResult;
        }
        
        var result = await ValidateTestState(test, ct);

        if (!result.IsValid)
        {
            return result;
        }
        
        await uow.SaveChanges(ct);
        await cache.RemoveCache(result.Value!.Id, ct);

        return result;

    }

    public async Task<TeczterValidationResult<TestEntity>> AddTestStep(TestEntity test, CreateTestStepRequestDto testStep, CancellationToken ct)
    {
        var testResult = composer.UsingContext(test)
            .AddStep(testStep)
            .Build();

        if (!testResult.IsValid)
        {
            return testResult;
        }
        
        var result = await ValidateTestState(test, ct);

        if (!result.IsValid)
        {
            return result;
        }
        
        await uow.SaveChanges(ct);
        await cache.RemoveCache(result.Value!.Id, ct);

        return result;

    }

    public async Task<TeczterValidationResult<TestEntity>> CreateNewTest(CreateTestRequestDto request, CancellationToken ct)
    {
        //Set created by user ID when users implemented.
        var testResult = composer
            .SetTitle(request.Title)
            .SetDescription(request.Description)
            .SetOwningDepartment(request.OwningDepartment)
            .AddLinkUrls(request.LinkUrls ?? [])
            .AddSteps(request.TestSteps)
            .Build();

        if (!testResult.IsValid)
        {
            return testResult;
        }
        
        var result = await ValidateTestState(testResult.Value!, ct);

        if (!result.IsValid)
        {
            return result;
        }
        
        await testAdapter.AddNewTest(testResult.Value!, ct);
        await uow.SaveChanges(ct);

        return result;

    }

    public async Task DeleteTest(TestEntity test, CancellationToken ct)
    {
        test.Delete();

        var executions = await executionAdapter.GetExecutionsForTest(test.Id);
        var executionsToDelete = executions.Where(x => x.ExecutionState is ExecutionStateType.Untested);

        foreach (var execution in executionsToDelete)
        {
            execution.Delete();
        }

        await uow.SaveChanges(ct);
        await cache.RemoveCache(test.Id, ct);
    }

    public async Task<TestEntity?> GetTestById(int id, CancellationToken ct)
    {
        var test = await cache.GetCachedResult(id, ct);

        if (test is not null)
        {
            return test;
        }

        test = await testAdapter.GetTestById(id, ct);

        if (test is not null)
        {
            await cache.SetCache(test, ct);
        }

        return test;
    }

    public async Task<List<TestEntity>> GetTestSearchResults(int pageNumber, string? testTitle, string? owningDepartment, CancellationToken ct)
    {
        var testSearchQuery = testAdapter.GetTestSearchBaseQuery();

        if (testTitle is not null)
        {
            testSearchQuery = testSearchQuery.Where(x => x.Title.Contains(testTitle));
        }
        
        if (owningDepartment is not null)
        {
            testSearchQuery = testSearchQuery.Where(x => x.OwningDepartment.ToString() == owningDepartment);
        }

        testSearchQuery = testSearchQuery.OrderBy(x => x.Title);
        testSearchQuery = testSearchQuery.Skip((pageNumber - 1) * TestsPerPage).Take(TestsPerPage);

        return await testSearchQuery.ToListAsync(ct);
    }

    public async Task<TeczterValidationResult<TestEntity>> RemoveLinkUrl(TestEntity test, string url, CancellationToken ct)
    {
        var testResult = test.RemoveLinkUrl(url);

        if (!testResult.IsValid)
        {
            return testResult;
        }
        
        var result = await ValidateTestState(test, ct);

        if (!result.IsValid)
        {
            return result;
        }
        
        await uow.SaveChanges(ct);
        await cache.RemoveCache(result.Value!.Id, ct);

        return result;

    }

    public async Task<TeczterValidationResult<TestEntity>> RemoveTestStep(TestEntity test, int testStepId, CancellationToken ct)
    {
        var testResult = test.RemoveTestStep(testStepId);

        if (!testResult.IsValid)
        {
            return testResult;
        }
        
        var result = await ValidateTestState(test, ct);

        if (!result.IsValid)
        {
            return result;
        }
        
        await uow.SaveChanges(ct);
        await cache.RemoveCache(result.Value!.Id, ct);

        return result;

    }

    public async Task<TeczterValidationResult<TestEntity>> UpdateTest(TestEntity test, UpdateTestRequestDto testUpdates, CancellationToken ct)
    {
        var testResult = composer.UsingContext(test)
            .SetTitle(testUpdates.Title)
            .SetDescription(testUpdates.Description)
            .SetOwningDepartment(testUpdates.OwningDepartment)
            .Build();

        if (!testResult.IsValid)
        {
            return testResult;
        }
        
        var result = await ValidateTestState(test, ct);

        if (!result.IsValid)
        {
            return result;
        }
        
        await uow.SaveChanges(ct);
        await cache.RemoveCache(result.Value!.Id, ct);

        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> UpdateTestStep(TestEntity test, int testStepId, UpdateTestStepRequestDto request, CancellationToken ct)
    {
        var testStep = test.TestSteps.Single(x => x.Id == testStepId && !x.IsDeleted);

        testStep.Update(request.StepPlacement, request.Instructions, request.Urls);

        test.SetCorrectStepPlacementValuesOnUpdate();

        var result = await ValidateTestState(test, ct);

        if (!result.IsValid)
        {
            return result;
        }
        
        await uow.SaveChanges(ct);
        await cache.RemoveCache(result.Value!.Id, ct);

        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> ValidateTestState(TestEntity test, CancellationToken ct)
    {
        var result = await testValidator.ValidateAsync(test, ct);

        if (!result.IsValid)
        {
            return TeczterValidationResult<TestEntity>.Fail(result.Errors.Select(x => x.ErrorMessage).ToArray());
        }

        return TeczterValidationResult<TestEntity>.Succeed(test);
    }
}