using Microsoft.EntityFrameworkCore;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Domain.ValueObjects;

namespace Teczter.Adapters;

public class TestStepAdapter(TeczterDbContext _dbContext) : ITestStepAdapter
{
    public async Task<TestStepEntity?> GetTestStepById(int id)
    {
        return await _dbContext.TestSteps
            .Include(x => x.Test)
            .Include(x => x.LinkUrls)
            .SingleOrDefaultAsync(x => x.Id == id);
    }
}
