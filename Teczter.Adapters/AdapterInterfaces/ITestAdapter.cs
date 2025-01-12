using Teczter.Domain.Entities;

namespace Teczter.Adapters.AdapterInterfaces;

public interface ITestAdapter
{
    Task CreateNewTest(TestEntity test);
    IQueryable<TestEntity> GetDetailedTestSearchBaseQuery();
    IQueryable<TestEntity> GetBasicTestSearchBaseQuery();
    Task<TestEntity?> GetTestById(Guid id);
    Task<TestEntity> UpdateTest(TestEntity test);
}