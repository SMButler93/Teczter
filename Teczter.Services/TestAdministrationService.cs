using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain.Entities;
using Teczter.Services.Dtos.RequestDtos.TestRequests;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class TestAdministrationService : ITestAdministrationService
{
    private readonly ITestAdministrationAdapter _testAdministrationAdapter;
    private readonly ITestRoundAdapter _testRoundAdapter;
    private readonly IUserAdapter _userAdapter;

    public TestAdministrationService(
        ITestAdministrationAdapter testAdministrationAdapter,
        ITestRoundAdapter testRoundAdapter,
        IUserAdapter userAdapter)
    {
        _testAdministrationAdapter = testAdministrationAdapter;
        _testRoundAdapter = testRoundAdapter;
        _userAdapter = userAdapter;
    }

    public Task CreateNewTest(TestEntity test)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTest(TestEntity test)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TestEntity>> GetTestSearchResults(TestSearchRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<TestEntity?> UpdateTest(TestEntity test)
    {
        throw new NotImplementedException();
    }
}
