using Teczter.Domain.Entities;

namespace Teczter.Adapters.AdapterInterfaces;

public interface ITestAdapter
{
    Task CreateNewTest(TestEntity test);
    IQueryable<TestEntity> GetTestSearchBaseQuery();
}