using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain.Entities;
using Teczter.Services.Dtos.RequestDtos.TestRequests;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class TestService : ITestService
{
    private readonly ITestAdapter _testAdapter;

    public TestService(
        ITestAdapter testAdapter)
    {
        _testAdapter = testAdapter;
    }

    public Task CreateNewTest(TestEntity test)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTest(TestEntity test)
    {
        throw new NotImplementedException();
    }

    public async Task<TestEntity?> GetTestById(Guid id)
    {
        return await _testAdapter.GetTestById(id);
    }

    public async Task<List<TestEntity>> GetTestSearchResults(string? testTitle, string? pillarOwner)
    {
        var TestSearchQuery = _testAdapter.GetTestSearchBaseQuery();

        TestSearchQuery = testTitle == default ? TestSearchQuery : TestSearchQuery.Where(x => x.Title.Contains(testTitle));
        TestSearchQuery = pillarOwner == default ? TestSearchQuery : TestSearchQuery.Where(x => x.Pillar == pillarOwner);

        return await TestSearchQuery.ToListAsync();
    }

    public Task<TestEntity?> UpdateTest(TestEntity test)
    {
        throw new NotImplementedException();
    }
}
