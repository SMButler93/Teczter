using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain.Entities;
using Teczter.Persistence;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class TestService : ITestService
{
    private readonly ITestAdapter _testAdapter;
    private readonly IUnitOfWork _uow;

    public TestService(
        ITestAdapter testAdapter,
        IUnitOfWork uow)
    {
        _testAdapter = testAdapter;
        _uow = uow;
    }

    public async Task<TestEntity> CreateNewTest(TestEntity test)
    {
        await _testAdapter.CreateNewTest(test);
        await _uow.CommitChanges();

        return test;
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
        var TestSearchQuery = _testAdapter.GetDetailedTestSearchBaseQuery();

        TestSearchQuery = testTitle == default ? TestSearchQuery : TestSearchQuery.Where(x => x.Title.Contains(testTitle));
        TestSearchQuery = pillarOwner == default ? TestSearchQuery : TestSearchQuery.Where(x => x.OwningPillar == pillarOwner);

        return await TestSearchQuery.ToListAsync();
    }

    public async Task<TestEntity> UpdateTest(TestEntity currentTest, TestEntity update)
    {
        var revisedTest = UpdateTestValues(currentTest, update);

        await _uow.CommitChanges();

        return revisedTest;
    }

    private TestEntity UpdateTestValues(TestEntity currentTest, TestEntity update)
    {
        currentTest.RevisedOn = DateTime.Now;

        currentTest.Title = update.Title;
        currentTest.Description = update.Title;
        currentTest.OwningPillar = update.OwningPillar;

        return currentTest;
    }
}