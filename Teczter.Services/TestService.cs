using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain.Entities;
using Teczter.Domain.ValidationObjects;
using Teczter.Domain.ValueObjects;
using Teczter.Persistence;
using Teczter.Services.Builders;
using Teczter.Services.DTOs.Request;
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
        ITestStepService testStepService,
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
        var linkUrl = new LinkUrl(url);

        _builder.UsingContext(test)
            .AddLinkUrl(linkUrl);

        var result = await ValidateTestState(test);

        if (!result.IsValid) { _uow.Rollback(); }
        if (result.IsValid) { await _uow.CommitChanges(); }

        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> AddTestStep(TestEntity test, TestStepCommandRequestDto testStep)
    {
        _builder.UsingContext(test)
            .AddStep(testStep);

        var result = await ValidateTestState(test);

        if (!result.IsValid) { _uow.Rollback(); }
        if (result.IsValid) { await _uow.CommitChanges(); }

        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> CreateNewTest(TestCommandRequestDto request)
    {
        var test = _builder
            .NewInstance()
            .SetTitle(request.Title)
            .SetDescription(request.Description)
            .SetPillarOwner(request.OwningPillar)
            .AddLinkUrls(request.LinkUrls)
            .AddSteps(request.TestSteps)
            .Build();

        var result = await ValidateTestState(test);

        if (!result.IsValid) { _uow.Rollback(); }
        if (result.IsValid) 
        {
            await _testAdapter.CreateNewTest(test);
            await _uow.CommitChanges(); 
        }

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

    public async Task<List<TestEntity>> GetTestSearchResults(string? testTitle, string? pillarOwner)
    {
        var TestSearchQuery = _testAdapter.GetBasicTestSearchBaseQuery();
        
        TestSearchQuery = testTitle == null ? TestSearchQuery : TestSearchQuery.Where(x => x.Title.Contains(testTitle));
        TestSearchQuery = pillarOwner == null ? TestSearchQuery : TestSearchQuery.Where(x => x.OwningPillar == pillarOwner);

        return await TestSearchQuery.ToListAsync();
    }

    public async Task<TeczterValidationResult<TestEntity>> RemoveLinkUrl(TestEntity test, string url)
    {
        var linkUrl = test.LinkUrls.SingleOrDefault(x => x.Url == url) ?? 
            throw new InvalidOperationException("Cannot remove a link that does not belong to this test");

        test.RemoveLinkUrl(linkUrl);

        var result = await ValidateTestState(test);

        if (!result.IsValid) { _uow.Rollback(); }
        if (result.IsValid) { await _uow.CommitChanges(); }

        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> RemoveTestStep(TestEntity test, Guid testStepId)
    {
        var testStep = test.TestSteps.SingleOrDefault(s => s.Id == testStepId) ?? 
            throw new InvalidOperationException("Cannot Remove a test step that does not exist, or does not belong to this test.");

        test.RemoveTestStep(testStep);
        var result = await ValidateTestState(test);

        if (!result.IsValid) { _uow.Rollback(); }
        if (result.IsValid) { await _uow.CommitChanges(); }

        return result;
    }

    public async Task<TeczterValidationResult<TestEntity>> UpdateTest(TestEntity test, TestCommandRequestDto testUpdates)
    {
        _builder.UsingContext(test)
            .SetTitle(testUpdates.Title)
            .SetDescription(testUpdates.Description)
            .SetPillarOwner(testUpdates.OwningPillar);

        var result = await ValidateTestState(test);

        if (!result.IsValid) { _uow.Rollback(); }
        if(result.IsValid) { await _uow.CommitChanges(); }

        return result;
    }

    private async Task<TeczterValidationResult<TestEntity>> ValidateTestState(TestEntity test)
    {
        var result = await _testValidator.ValidateAsync(test);

        if (!result.IsValid)
        {
            return TeczterValidationResult<TestEntity>.Fail(result.Errors.Select(x => x.ErrorMessage).ToArray());
        }

        return TeczterValidationResult<TestEntity>.Succeed(test);
    }
}