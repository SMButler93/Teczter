using Teczter.Domain.Entities;

namespace Teczter.Services.ValidationRepositoryInterfaces
{
    public interface ITestValidationRepository
    {
        Task<List<TestEntity>> GetTestEntitiesWithTitle(string title);
    }
}
