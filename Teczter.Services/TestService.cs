using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain.Entities;
using Teczter.Domain.Exceptions;
using Teczter.Domain.ValidationObjects;
using Teczter.Persistence;
using Teczter.Services.DTOs.Request;
using Teczter.Services.RequestDtos.Request;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class TestService : ITestService
{
    private readonly ITestAdapter _testAdapter;
    private readonly ITestBuilder _builder;
    private readonly IUnitOfWork _uow;
    private readonly IValidator<TestEntity> _testValidator;

    public TestService(
        ITestAdapter testAdapter,
        ITestBuilder builder,
        IUnitOfWork uow,
        IValidator<TestEntity> testValidator)
    {
        _testAdapter = testAdapter;
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

    public async Task<TeczterValidationResult<TestEntity>> AddTestStep(TestEntity test, TestStepCommandRequestDto testStep)
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
            .AddLinkUrls(request.LinkUrls)
            .AddSteps(request.TestSteps)
            .Build();

        var result = await ValidateTestState(test);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task DeleteTest(TestEntity test)
    {
        test.Delete();
        await _uow.CommitChanges();
    }

    public async Task<TestEntity?> GetTestById(Guid id)
    {
        return await _testAdapter.GetTestById(id);
    }

    public async Task<List<TestEntity>> GetTestSearchResults(string? testTitle, string? owningDepartment)
    {
        var TestSearchQuery = _testAdapter.GetBasicTestSearchBaseQuery();
        
        TestSearchQuery = testTitle == null ? TestSearchQuery : TestSearchQuery.Where(x => x.Title.Contains(testTitle));
        TestSearchQuery = owningDepartment == null ? TestSearchQuery : TestSearchQuery.Where(x => x.OwningDepartment == owningDepartment);

        return await TestSearchQuery.ToListAsync();
    }

    public async Task<TeczterValidationResult<TestEntity>> RemoveLinkUrl(TestEntity test, string url)
    {
        var linkUrl = test.LinkUrls.SingleOrDefault(x => x.Url == url) ?? 
            throw new InvalidOperationException("Cannot remove a link that does not belong to this test");

        test.RemoveLinkUrl(linkUrl);

        var result = await ValidateTestState(test);

        await EvaluateValidationResultAndPersist(result);
        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> RemoveTestStep(TestEntity test, Guid testStepId)
    {
        var testStep = test.TestSteps.SingleOrDefault(s => s.Id == testStepId) ?? 
            throw new TeczterValidationException("Cannot Remove a test step that does not exist, or does not belong to this test.");

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