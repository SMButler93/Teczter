using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;
using Teczter.Adapters;
using Teczter.Data;
using Teczter.Domain.Entities;

namespace Teczter.Adapter.Tests;

[TestFixture]
public class ExecutionGroupAdapterTests
{
    private DbContextOptions<TeczterDbContext> _dbOptions = null!;
    private TeczterDbContext _dbContext = null!;
    private ExecutionGroupAdapter _sut = null!;

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
        await _dbContext.ExecutionGroups.AddRangeAsync(GetMultipleExecutionGroupInstances());
        await _dbContext.SaveChangesAsync();

        _sut = new(_dbContext);
    }

    [Test]
    public async Task GetExecutionGroupById_WhenExistsAndNotDeleted_ShouldReturnInstance()
    {
        //Arrange:
        var executionGroupToGet = GetMultipleExecutionGroupInstances().First();
        var ct = new CancellationTokenSource().Token;

        //Act:
        var result = await _sut.GetExecutionGroupById(executionGroupToGet.Id, ct);

        //Assert:
        result.ShouldNotBeNull();
        result.Id.ShouldBe(executionGroupToGet.Id);
        result.CreatedById.ShouldBe(executionGroupToGet.CreatedById);
        result.RevisedById.ShouldBe(executionGroupToGet.RevisedById);
        result.ExecutionGroupName.ShouldBe(executionGroupToGet.ExecutionGroupName);
        result.SoftwareVersionNumber.ShouldBe(executionGroupToGet.SoftwareVersionNumber);
    }

    [Test]
    public async Task GetExecutionGroupById_WhenDeleted_ShouldReturnNull()
    {
        //Arrange:
        var executionGroupToGet = GetMultipleExecutionGroupInstances().First();
        var trackedExecutionGroup = await _dbContext.ExecutionGroups.SingleAsync(x => x.Id == executionGroupToGet.Id);
        var ct = new CancellationTokenSource().Token;
        trackedExecutionGroup.Delete();
        await _dbContext.SaveChangesAsync();

        //Act:
        var result = await _sut.GetExecutionGroupById(executionGroupToGet.Id, ct);

        //Assert:
        result.ShouldBeNull();
    }

    [Test]
    public void GetBasicExecutionGroupSearchQuery_WhenCalled_ShouldReturnQueryable()
    {
        //Arrange & Act:
        var result = _sut.GetBasicExecutionGroupSearchQuery();

        //Assert:
        result.ShouldBeAssignableTo<IQueryable<ExecutionGroupEntity>>();
    }

    [Test]
    public async Task GetBasicExecutionGroupSearchQuery_WhenExecuted_ShouldReturnAllNonDeletedExecutionGroups()
    {
        //Arrange & Act:
        var expectedGroups = GetMultipleExecutionGroupInstances();
        var query = _sut.GetBasicExecutionGroupSearchQuery();
        var result = await query.ToListAsync();

        //Assert:
        result.Count.ShouldBe(expectedGroups.Count);
        result.ShouldAllBe(x => !x.IsDeleted);
    }

    [Test]
    public async Task CreateNewExecutionGroup_WhenExecuted_ShouldAddNewInstanceToDatabase()
    {
        //Arrange:
        var ct = new CancellationTokenSource().Token;
        var executionGroup = new ExecutionGroupEntity()
        {
            Id = 5,
            CreatedById = 1,
            RevisedById = 1,
            ExecutionGroupName = "Group 5",
            SoftwareVersionNumber = "1.1.5",
        };

        //Act:
        var initialPersistedExecutionGroupsCount = await _dbContext.ExecutionGroups.CountAsync();
        await _sut.AddNewExecutionGroup(executionGroup, ct);
        await _dbContext.SaveChangesAsync();
        var persistedExecutionGroups = await _dbContext.ExecutionGroups.ToListAsync();
        var newlyPersistedExecutionGroup = await _dbContext.ExecutionGroups.SingleAsync(x => x.Id == executionGroup.Id);

        //Assert:
        persistedExecutionGroups.Select(x => x.Id).ShouldContain(executionGroup.Id);
        persistedExecutionGroups.Count.ShouldBeGreaterThan(initialPersistedExecutionGroupsCount);
        newlyPersistedExecutionGroup.ExecutionGroupName.ShouldBe(executionGroup.ExecutionGroupName);
        newlyPersistedExecutionGroup.SoftwareVersionNumber.ShouldBe(executionGroup.SoftwareVersionNumber);
    }

    private static List<ExecutionGroupEntity> GetMultipleExecutionGroupInstances()
    {
        return
        [
            new ExecutionGroupEntity()
            {
                Id = 1,
                CreatedById = 1,
                RevisedById = 1,
                ExecutionGroupName = "Group 1",
                SoftwareVersionNumber = "1.1.1",
            },
            new ExecutionGroupEntity()
            {
                Id = 2,
                CreatedById = 1,
                RevisedById = 1,
                ExecutionGroupName = "Group 2",
                SoftwareVersionNumber = "1.1.2",
            },
            new ExecutionGroupEntity()
            {
                Id = 3,
                CreatedById = 1,
                RevisedById = 1,
                ExecutionGroupName = "Group 3",
                SoftwareVersionNumber = "1.1.3",
            },
            new ExecutionGroupEntity()
            {
                Id = 4,
                CreatedById = 1,
                RevisedById = 1,
                ExecutionGroupName = "Group 4",
                SoftwareVersionNumber = "1.1.4",
            }
        ];
    }
}