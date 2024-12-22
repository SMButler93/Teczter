using Teczter.Domain.Entities;

namespace Teczter.Adapters.AdapterInterfaces;

public interface ITestAdministrationAdapter
{
    Task CreateNewTest(TestEntity test);
    IQueryable<TestEntity> GetTestSearchBaseQuery();
}