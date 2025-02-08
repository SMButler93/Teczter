using Teczter.Domain.Entities;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestStepService
{
    Task<TestStepEntity?> GetTestStepById(Guid id);
}
