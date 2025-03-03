using Teczter.Domain.Entities;
using Teczter.Domain.ValueObjects;

namespace Teczter.Adapters.AdapterInterfaces;

public interface ITestStepAdapter
{
    Task<TestStepEntity?> GetTestStepById(int id);
}
