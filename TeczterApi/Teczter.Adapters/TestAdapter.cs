using Microsoft.EntityFrameworkCore;
using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Services.AdapterInterfaces;

namespace Teczter.Adapters;

public class TestAdapter(TeczterDbContext _dbContext) : ITestAdapter
{
    private const int TestsPerPage = 15;
    
    public async Task AddNewTest(TestEntity test, CancellationToken ct)
    {
        await _dbContext.Tests.AddAsync(test, ct);
    }

    public async Task<TestEntity?> GetTestById(int id, CancellationToken ct)
    {
        return await _dbContext.Tests
            .Include(x => x.TestSteps.OrderBy(y => y.StepPlacement))
            .SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }

    public async Task<List<TestEntity>> GetTestSearchResults(int pageNumber, string? testTitle, string? owningDepartment, CancellationToken ct)
     {
        var query = _dbContext.Tests
            .AsNoTracking()
            .Include(x => x.TestSteps)
            .Where(x => !x.IsDeleted);
        
        if (testTitle is not null)
        {
            query = query.Where(x => x.Title.Contains(testTitle));
        }
        
        if (owningDepartment is not null)
        {
            query = query.Where(x => x.OwningDepartment.ToString() == owningDepartment);
        }

        query = query.OrderBy(x => x.Title);
        query = query.Skip((pageNumber - 1) * TestsPerPage).Take(TestsPerPage);

        return await query.ToListAsync(ct);
    }
}