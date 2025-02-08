using Teczter.Adapters.AdapterInterfaces;
using Teczter.Domain.Entities;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services;

public class TestStepService : ITestStepService
{
    private readonly ITestStepAdapter _testStepAdapter;

    public TestStepService(ITestStepAdapter testStepAdapter)
    {
        _testStepAdapter = testStepAdapter;
    }
    public async Task<TestStepEntity?> GetTestStepById(Guid id)
    {
        return await _testStepAdapter.GetTestStepById(id);
    }
}
