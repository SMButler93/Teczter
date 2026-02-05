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

        await SetupSubjectUnderTest();
    }

    private async Task SetupSubjectUnderTest()
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
        var result = await _sut.GetExecutionById(executionToGet.Id,  CancellationToken.None);

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
        var result = await _sut.GetExecutionById(executionToGetId,  CancellationToken.None);

        //Assert:
        result.ShouldBeNull();
    }

    [Test]
    public async Task GetExecutionById_WhenDoesNotExist_ShouldReturnNull()
    {
        //Arrange:
        var nonExistentId = GetMultipleExecutionInstances().OrderBy(x => x.Id).Last().Id + 1;

        //Act:
        var result = await _sut.GetExecutionById(nonExistentId,  CancellationToken.None);

        //Assert:
        result.ShouldBeNull();
    }

    [Test]
    public async Task GetExecutionsForTest_WhenExists_ShouldGetAllInstancesForThatTest()
    {
        //Arrange:
        var testId = GetMultipleExecutionInstances().First().TestId;
        var matchingExecutions = GetMultipleExecutionInstances().Where(x => x.TestId == testId).ToList();

        //Act:
        var result = await _sut.GetExecutionsForTest(testId,  CancellationToken.None);

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
        var result = await _sut.GetExecutionsForTest(testId,  CancellationToken.None);

        //Assert:
        result.Count.ShouldBe(0);
    }

    private static List<ExecutionEntity> GetMultipleExecutionInstances()
    {
        var test1 = new TestEntity() { Id = 1 };
        var test2 = new TestEntity() { Id = 2 };
        
        return
        [
            new ExecutionEntity()
            {
                Id = 1,
                CreatedById = 1,
                RevisedById = 1,
                ExecutionGroupId = 1,
                TestId = 1,
                ExecutionGroup = new ExecutionGroupEntity
                {
                    Id = 1,
                    ExecutionGroupName = "Group 1",
                    SoftwareVersionNumber = "1.1.1"
                },
                Test = test1
            },
            new ExecutionEntity()
            {
                Id = 2,
                CreatedById = 1,
                RevisedById = 1,
                ExecutionGroupId = 1,
                TestId = 1,
                ExecutionGroup = new ExecutionGroupEntity
                {
                    Id = 2,
                    ExecutionGroupName = "Group 2",
                    SoftwareVersionNumber = "1.1.2"
                },
                Test = test1
            },
            new ExecutionEntity()
            {
                Id = 3,
                CreatedById = 1,
                RevisedById = 1,
                ExecutionGroupId = 1,
                TestId = 2,
                ExecutionGroup = new ExecutionGroupEntity
                {
                    Id = 3,
                    ExecutionGroupName = "Group 3",
                    SoftwareVersionNumber = "1.1.3"
                },
                Test = test2
            },
            new ExecutionEntity()
            {
                Id = 4,
                CreatedById = 1,
                RevisedById = 1,
                ExecutionGroupId = 1,
                TestId = 2,
                ExecutionGroup = new ExecutionGroupEntity
                {
                    Id = 4,
                    ExecutionGroupName = "Group 4",
                    SoftwareVersionNumber = "1.1.4"
                },
                Test = test2
            }
        ];
    }
}
