using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Data;
using Teczter.Domain.Entities;

namespace Teczter.Adapters;

public class TestStepAdapter(TeczterDbContext _dbContext) : ITestStepAdapter
{
    public async Task<TestStepEntity?> GetTestStepById(int id)
    {
        return await _dbContext.TestSteps
            .Include(x => x.Urls)
            .SingleOrDefaultAsync(x => x.Id == id);
    }
}
