using Teczter.Domain.Entities;

namespace Teczter.Services.AdapterInterfaces;

public interface ITestAdapter
{
    Task AddNewTest(TestEntity test, CancellationToken ct);

    Task<List<TestEntity>> GetTestSearchResults(int pageNumber, string? testTitle, string? owningDepartment,
        CancellationToken ct);
    Task<TestEntity?> GetTestById(int id, CancellationToken ct);
}