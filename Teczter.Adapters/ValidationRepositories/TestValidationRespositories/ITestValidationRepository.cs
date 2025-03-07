using Teczter.Domain.Entities;

namespace Teczter.Adapters.ValidationRepositories.TestValidationRespositories
{
    public interface ITestValidationRepository
    {
        List<TestEntity> GetTestEntitiesWithTitle(string title);
    }
}
