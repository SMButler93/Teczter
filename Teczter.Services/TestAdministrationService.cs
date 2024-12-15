using Teczter.Adapters;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain.Entities;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class TestAdministrationService : ITestAdministrationService
{
    private readonly ITestAdministrationAdapter _testAdministrationAdapter;
    private readonly ITestRoundAdapter _testRoundAdapter;

    public TestAdministrationService(
        ITestAdministrationAdapter testAdministrationAdapter,
        ITestRoundAdapter testRoundAdapter)
    {
        _testAdministrationAdapter = testAdministrationAdapter;
        _testRoundAdapter = testRoundAdapter;
    }

    public Task CreateNewTest(TestEntity test)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTest(TestEntity test)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TestEntity>> GetTestsInTestRound(string testRoundName)
    {
        var testRound = await _testRoundAdapter.GetTestRoundByTestRoundName(testRoundName);

        if (testRound == null)
        {
            throw new ArgumentException($"Test round {testRoundName} does not exist.");
        }

        return testRound.Tests;
    }

    public Task<List<TestEntity>> GetAllUsersAssignedTests(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<TestEntity?> GetTestById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<TestEntity?> GetTestByName(string testRoundName)
    {
        throw new NotImplementedException();
    }

    public Task<TestEntity?> UpdateTest(TestEntity test)
    {
        throw new NotImplementedException();
    }
}
