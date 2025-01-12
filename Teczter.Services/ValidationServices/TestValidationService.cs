using Teczter.Adapters.AdapterInterfaces;
using Teczter.Services.ValidationServices.Interfaces;

namespace Teczter.Services.ValidationServices;

public class TestValidationService(ITestAdapter _testAdapter) : ITestValidationService
{
    public bool HasUniqueTitle(string testTitle)
    {
        return !_testAdapter.GetBasicTestSearchBaseQuery().Any(x => x.Title == testTitle);
    }
}
