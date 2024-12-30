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

    public async Task CreateNewTest(TestEntity test)
    {
        try
        {
            await _testAdapter.CreateNewTest(test);
            await _uow.CommitChanges();
        }
        catch (DbUpdateException)
        {
            throw new InvalidOperationException("Failed to create the test");
        }
    }

    public async Task DeleteTest(Guid id)
    {
        var test = await _testAdapter.GetTestById(id) ?? throw new InvalidOperationException("Cannot delete a test that does not exist.");

        try
        {
            test.Delete();
            await _uow.CommitChanges();
        }
        catch (DbUpdateException dbEx)
        {
            throw new InvalidOperationException("Failed to delete the test.", dbEx);
        }
    }

    public async Task<TestEntity?> GetTestById(Guid id)
    {
        return await _testAdapter.GetTestById(id);
    }

    public async Task<List<TestEntity>> GetTestSearchResults(string? testTitle, string? pillarOwner)
    {
        var TestSearchQuery = _testAdapter.GetTestSearchBaseQuery();

        TestSearchQuery = testTitle == null ? TestSearchQuery : TestSearchQuery.Where(x => x.Title.Contains(testTitle));
        TestSearchQuery = pillarOwner == null ? TestSearchQuery : TestSearchQuery.Where(x => x.Pillar == pillarOwner);

        return await TestSearchQuery.ToListAsync();
    }

    public Task<TestEntity?> UpdateTest(TestEntity test)
    {
        throw new NotImplementedException();
    }
}
