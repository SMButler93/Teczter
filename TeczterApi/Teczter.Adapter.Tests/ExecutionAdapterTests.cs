using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;
using Teczter.Adapters;
using Teczter.Data;
using Teczter.Domain.Entities;

namespace Teczter.Adapter.Tests;

[TestFixture]
public class ExecutionAdapterTests
{
    private DbContextOptions<TeczterDbContext> _dbOptions = null!;
    private TeczterDbContext _dbContext = null!;
    private ExecutionAdapter _sut = null!;

    [SetUp]
    public async Task Setup()
    {
        _dbOptions = new DbContextOptionsBuilder<TeczterDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        await SubjectUnderTest();
    }

    private async Task SubjectUnderTest()
    {
        _dbContext = new TeczterDbContext(_dbOptions);
        await _dbContext.Executions.AddRangeAsync(GetMultipleExecutionInstances());
        await _dbContext.SaveChangesAsync();

        _sut = new(_dbContext);
    }

    [Test]
    public async Task GetExecutionById_WhenExistsAndNotDeleted_ReturnsExecution()
    {
        //Arrange:
        var executionToGet = GetMultipleExecutionInstances().First();

        //Act:
        var result = await _sut.GetExecutionById(executionToGet.Id);

        //Assert:
        result.ShouldNotBeNull();
        result.Id.ShouldBe(executionToGet.Id);
        result.IsDeleted.ShouldBeFalse();
        result.CreatedById.ShouldBe(executionToGet.CreatedById);
        result.RevisedById.ShouldBe(executionToGet.RevisedById);
        result.TestId.ShouldBe(executionToGet.TestId);
    }

    [Test]
    public async Task GetExecutionById_WhenExistsButIsSoftDeleted_ShouldReturnNull()
    {
        //Arrange:
        var executionToGetId = GetMultipleExecutionInstances().First().Id;
        var trackedExecution = await _dbContext.Executions.SingleAsync(x => x.Id == executionToGetId);
        trackedExecution.Delete();
        await _dbContext.SaveChangesAsync();

        //Act:
        var result = await _sut.GetExecutionById(executionToGetId);

        //Assert:
        result.ShouldBeNull();
    }

    [Test]
    public async Task GetExecutionById_WhenDoesNotExist_ShouldReturnNull()
    {
        //Arrange:
        var nonExistentId = GetMultipleExecutionInstances().OrderBy(x => x.Id).Last().Id + 1;

        //Act:
        var result = await _sut.GetExecutionById(nonExistentId);

        //Assert:
        result.ShouldBeNull();
    }

    [Test]
    public async Task GetExecutionsForTest_WhenExists_ShouldGetAllInstances()
    {
        //Arrange:
        var testId = GetMultipleExecutionInstances().First().TestId;
        var matchingExecutions = GetMultipleExecutionInstances().Where(x => x.TestId == testId).ToList();

        //Act:
        var result = await _sut.GetExecutionsForTest(testId);

        //Assert:
        result.Count.ShouldBe(matchingExecutions.Count);
        result.ForEach(x => x.TestId.ShouldBe(testId));
    }

    [Test]
    public async Task GetExecutionsForTest_WhenDeletedOrDoNotExist_ShouldReturnEmptyArray()
    {
        //Arrange:
        var testId = GetMultipleExecutionInstances().First().TestId;
        var matchingExecutions = GetMultipleExecutionInstances().Where(x => x.TestId == testId).ToList();
        var trackedExecutions = await _dbContext.Executions.Where(x => matchingExecutions.Select(y => y.TestId).Contains(x.TestId)).ToListAsync();
        trackedExecutions.ForEach(x => x.Delete());
        await _dbContext.SaveChangesAsync();

        //Act:
        var result = await _sut.GetExecutionsForTest(testId);

        //Assert:
        result.Count.ShouldBe(0);
    }

    private List<ExecutionEntity> GetMultipleExecutionInstances()
    {
        return
        [
            new ExecutionEntity()
            {
                Id = 1,
                CreatedById = 1,
                RevisedById = 1,
                IsDeleted = false,
                ExecutionGroupId = 1,
                TestId = 1
            },
            new ExecutionEntity()
            {
                Id = 2,
                CreatedById = 1,
                RevisedById = 1,
                IsDeleted = false,
                ExecutionGroupId = 1,
                TestId = 1
            },
            new ExecutionEntity()
            {
                Id = 3,
                CreatedById = 1,
                RevisedById = 1,
                IsDeleted = false,
                ExecutionGroupId = 1,
                TestId = 2
            },
            new ExecutionEntity()
            {
                Id = 4,
                CreatedById = 1,
                RevisedById = 1,
                IsDeleted = false,
                ExecutionGroupId = 1,
                TestId = 2
            }
        ];
    }
}
