using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Teczter.Domain;
using Teczter.Domain.Entities;
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
        IValidator<TestEntity> _testValidator) : ITestService
{
    private const int testsPerPage = 15;

    public async Task<TeczterValidationResult<TestEntity>> AddLinkUrl(TestEntity test, string url)
    {
        var testResult = _composer.UsingContext(test)
            .AddLinkUrl(url)
            .ValidateInvariants();

        if (testResult.IsValid)
        {
            var result = await ValidateTestState(test);

            if (result.IsValid)
            {
                await _uow.CommitChanges();
            }

            return result;
        }

        return testResult;
    }

    public async Task<TeczterValidationResult<TestEntity>> AddTestStep(TestEntity test, CreateTestStepRequestDto testStep)
    {
        var testResult = _composer.UsingContext(test)
            .AddStep(testStep)
            .ValidateInvariants();

        if (testResult.IsValid)
        {
            var result = await ValidateTestState(test);

            if (result.IsValid)
            {
                await _uow.CommitChanges();
            }

            return result;
        }

        return testResult;
    }

    public async Task<TeczterValidationResult<TestEntity>> CreateNewTest(CreateTestRequestDto request)
    {
        //Set created by user ID when users implemented.
        var testResult = _composer
            .SetTitle(request.Title)
            .SetDescription(request.Description)
            .SetOwningDepartment(request.OwningDepartment)
            .AddLinkUrls(request.LinkUrls ?? [])
            .AddSteps(request.TestSteps)
            .Build();

        if (testResult.IsValid)
        {
            await _testAdapter.AddNewTest(testResult.Value!);

            var result = await ValidateTestState(testResult.Value!);

            if (result.IsValid)
            {
                await _uow.CommitChanges();
            }

            return result;
        }

        return testResult;
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

    public async Task<List<TestEntity>> GetTestSearchResults(int pageNumber, string? testTitle, string? owningDepartment)
    {
        var testSearchQuery = _testAdapter.GetTestSearchBaseQuery();

        if (testTitle is not null)
        {
            testSearchQuery = testSearchQuery.Where(x => x.Title.Contains(testTitle));
        }
        
        if (owningDepartment is not null)
        {
            testSearchQuery = testSearchQuery.Where(x => x.OwningDepartment.ToString() == owningDepartment);
        }

        testSearchQuery = testSearchQuery.OrderBy(x => x.Title);
        testSearchQuery = testSearchQuery.Skip((pageNumber - 1) * testsPerPage).Take(testsPerPage);

        return await testSearchQuery.ToListAsync();
    }

    public async Task<TeczterValidationResult<TestEntity>> RemoveLinkUrl(TestEntity test, string url)
    {
        var testResult = test.RemoveLinkUrl(url);

        if (testResult.IsValid)
        {
            var result = await ValidateTestState(test);

            if (result.IsValid)
            {
                await _uow.CommitChanges();
            }

            return result; 
        }

        return testResult;
    }

    public async Task<TeczterValidationResult<TestEntity>> RemoveTestStep(TestEntity test, int testStepId)
    {
        var testResult = test.RemoveTestStep(testStepId);

        if (testResult.IsValid)
        {
            var result = await ValidateTestState(test);

            if (result.IsValid)
            {
                await _uow.CommitChanges();
            }

            return result; 
        }

        return testResult;
    }

    public async Task<TeczterValidationResult<TestEntity>> UpdateTest(TestEntity test, UpdateTestRequestDto testUpdates)
    {
        var testResult = _composer.UsingContext(test)
            .SetTitle(testUpdates.Title)
            .SetDescription(testUpdates.Description)
            .SetOwningDepartment(testUpdates.OwningDepartment)
            .ValidateInvariants();

        if (testResult.IsValid)
        {
            var result = await ValidateTestState(test);

            if (result.IsValid)
            {
                await _uow.CommitChanges();
            }

            return result; 
        }

        return testResult; ;
    }

    public async Task<TeczterValidationResult<TestEntity>> UpdateTestStep(TestEntity test, int testStepId, UpdateTestStepRequestDto request)
    {
        var testStep = test.TestSteps.Single(x => x.Id == testStepId && !x.IsDeleted);

        testStep.Update(request.StepPlacement, request.Instructions, request.Urls);

        test.SetCorrectStepPlacementValuesOnUpdate();

        var result = await ValidateTestState(test);

        if (result.IsValid)
        {
            await _uow.CommitChanges();
        }

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
}