using Teczter.Domain.Entities;

namespace Teczter.Services.AdapterInterfaces;

public interface ITestAdapter
{
    Task AddNewTest(TestEntity test, CancellationToken ct);
    IQueryable<TestEntity> GetTestSearchBaseQuery();
    Task<TestEntity?> GetTestById(int id, CancellationToken ct);
}