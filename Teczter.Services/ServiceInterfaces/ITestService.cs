using Teczter.Domain.Entities;
using Teczter.Services.Dtos;
using Teczter.Services.Dtos.RequestDtos.TestRequests;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestService
{
    Task CreateNewTest(TestEntity test);
    Task<List<TestEntity>> GetTestSearchResults(string? testName, string? pillarOwner);
    Task DeleteTest(TestEntity test);
    Task<TestEntity?> UpdateTest(TestEntity test);
}
