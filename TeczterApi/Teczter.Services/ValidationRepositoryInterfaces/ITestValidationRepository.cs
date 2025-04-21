using Teczter.Domain.Entities;

namespace Teczter.Services.ValidationRepositoryInterfaces
{
    public interface ITestValidationRepository
    {
        List<TestEntity> GetTestEntitiesWithTitle(string title);
    }
}
