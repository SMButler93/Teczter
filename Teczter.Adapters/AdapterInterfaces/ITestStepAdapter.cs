using Teczter.Domain.Entities;

namespace Teczter.Adapters.AdapterInterfaces;

public interface ITestStepAdapter
{
    Task<TestStepEntity?> GetTestStepById(int id);
}
