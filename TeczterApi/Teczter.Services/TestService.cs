using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Domain.Exceptions;
using Teczter.Persistence;
using Teczter.Services.AdapterInterfaces;
using Teczter.Services.RequestDtos.Tests;
using Teczter.Services.RequestDtos.TestSteps;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class TestService : ITestService
{
    private readonly ITestAdapter _testAdapter;
    private readonly IExecutionAdapter _executionAdapter;
    private readonly ITestComposer _composer;
    private readonly IUnitOfWork _uow;
    private readonly IValidator<TestEntity> _testValidator;

    private const int testsPerPage = 15;

    public TestService(
        ITestAdapter testAdapter,
        IExecutionAdapter executionAdapter,
        ITestComposer composer,
        IUnitOfWork uow,
        IValidator<TestEntity> testValidator)
    {
        _testAdapter = testAdapter;
        _executionAdapter = executionAdapter;
        _composer = composer;
        _uow = uow;
        _testValidator = testValidator;
    }

    public async Task<TeczterValidationResult<TestEntity>> AddLinkUrl(TestEntity test, string url)
    {
        _composer.UsingContext(test)
            .AddLinkUrl(url);

        var result = await ValidateTestState(test);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> AddTestStep(TestEntity test, CreateTestStepRequestDto testStep)
    {
        _composer.UsingContext(test)
            .AddStep(testStep);

        var result = await ValidateTestState(test);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> CreateNewTest(CreateTestRequestDto request)
    {
        var test = _composer
            .SetTitle(request.Title)
            .SetDescription(request.Description)
            .SetOwningDepartment(request.OwningDepartment)
            .AddLinkUrls(request.LinkUrls ?? [])
            .AddSteps(request.TestSteps)
            .Build();

        await _testAdapter.AddNewTest(test);

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

    public async Task<List<TestEntity>> GetTestSearchResults(int pageNumber, string? testTitle, string? owningDepartment)
    {
        var testSearchQuery = _testAdapter.GetBasicTestSearchBaseQuery();

        if (testTitle != null)
        {
            testSearchQuery = testSearchQuery.Where(x => x.Title.Contains(testTitle));
        }
        
        if (owningDepartment != null)
        {
            testSearchQuery = testSearchQuery.Where(x => x.OwningDepartment.ToString() == owningDepartment);
        }

        testSearchQuery = testSearchQuery.OrderBy(x => x.Title);
        testSearchQuery = testSearchQuery.Skip((pageNumber - 1) * testsPerPage).Take(testsPerPage);

        return await testSearchQuery.ToListAsync();
    }

    public async Task<TeczterValidationResult<TestEntity>> RemoveLinkUrl(TestEntity test, string url)
    {
        test.RemoveLinkUrl(url);

        var result = await ValidateTestState(test);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> RemoveTestStep(TestEntity test, int testStepId)
    {
        test.RemoveTestStep(testStepId);

        var result = await ValidateTestState(test);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> UpdateTest(TestEntity test, UpdateTestRequestDto testUpdates)
    {
        _composer.UsingContext(test)
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
            throw new TeczterValidationException("Cannot update a test step that does not exist, has already been deleted " +
            "or does not belong to this test.");

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