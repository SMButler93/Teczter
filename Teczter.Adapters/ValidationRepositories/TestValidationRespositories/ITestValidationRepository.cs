using Teczter.Domain.Entities;

namespace Teczter.Adapters.ValidationRepositories.TestValidationRespositories
{
    public interface ITestValidationRepository
    {
        Task<List<TestEntity>> GetTestEntitiesWithTitle(string title);
    }
}
