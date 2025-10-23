using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;
using Teczter.Adapters;
using Teczter.Data;
using Teczter.Domain.Entities;
using Teczter.Domain.Enums;

namespace Teczter.Adapter.Tests;

[TestFixture]
public class TestAdapterTests
{
    private DbContextOptions<TeczterDbContext> _dbOptions = null!;
    private TeczterDbContext _dbContext = null!;
    private TestAdapter _sut = null!;

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
        await _dbContext.Tests.AddRangeAsync(GetMultipleBasicTestInstances());
        await _dbContext.SaveChangesAsync();

        _sut = new(_dbContext);
    }

    [Test]
    public async Task GetTestById_WhenExists_ShouldReturnTest()
    {
        //Arrange:
        var testToSearchFor = GetMultipleBasicTestInstances().First();

        //Act:
        var result = await _sut.GetTestById(testToSearchFor.Id);

        //Assert:
        result.ShouldNotBeNull();
        result.Id.ShouldBe(testToSearchFor.Id);
        result.Title.ShouldBe(testToSearchFor.Title);
        result.Description.ShouldBe(testToSearchFor.Description);
        result.OwningDepartment.ShouldBe(testToSearchFor.OwningDepartment);
        result.TestSteps.Count.ShouldBe(testToSearchFor.TestSteps.Count);
    }

    [Test]
    public async Task GetTestById_WhenDoesNotExists_ShouldReturnNull()
    {
        //Arrange:
        var nonExistentTestId = 123;

        //Act:
        var result = await _sut.GetTestById(nonExistentTestId);

        //Assert:
        result.ShouldBeNull();
    }

    [Test]
    public async Task CreateNewTest_WhenAdded_ShouldAppearInDatabase()
    {
        //Arrange:
        var testToAdd = GetSingleBasicTestInstance();
        var initialPersistedTestsCount = await _dbContext.Tests.CountAsync();

        //Act:
        await _sut.AddNewTest(testToAdd);
        await _dbContext.SaveChangesAsync();
        var persistedTestsWithNewEntry = await _dbContext.Tests.ToListAsync();

        //Assert:
        persistedTestsWithNewEntry.Count.ShouldBeGreaterThan(initialPersistedTestsCount);
        persistedTestsWithNewEntry.Select(x => x.Id).ShouldContain(testToAdd.Id);
    }

    [Test]
    public async Task GetBasicTestSearchQuery_WhenNoAdditionalFiltersAreApplied_ShouldReturnAllNonDeletedTests()
    {
        //Arrange:
        var preSeededTests = await _dbContext.Tests.ToListAsync();
        var testToMarkAsDeleted = preSeededTests.First();
        testToMarkAsDeleted.Delete();
        await _dbContext.SaveChangesAsync();

        //Act:
        var query = _sut.GetTestSearchBaseQuery();
        var results = await query.ToListAsync();

        //Assert:
        results.Count.ShouldBeLessThan(preSeededTests.Count);
        results.ShouldNotContain(x => x.Id == testToMarkAsDeleted.Id);
    }

    private static TestEntity GetSingleBasicTestInstance()
    {
        return new TestEntity()
        {
            Id = 999,
            Title = "test 999",
            Description = "test 999.",
            OwningDepartment = Department.Accounting,
            TestSteps = GetBasicTestStepInstances()
        };
    }

    private static List<TestEntity> GetMultipleBasicTestInstances()
    {
        return
        [
            new TestEntity()
            {
                Id = 1,
                Title = "One",
                Description = "test 1.",
                OwningDepartment = Department.Accounting,
                TestSteps = GetBasicTestStepInstances()
            },
            new TestEntity()
            {
                Id = 2,
                Title = "Two",
                Description = "test 2.",
                OwningDepartment = Department.Core,
                TestSteps = GetBasicTestStepInstances()
            },
            new TestEntity()
            {
                Id = 3,
                Title = "Three",
                Description = "test 3.",
                OwningDepartment = Department.Operations,
                TestSteps = GetBasicTestStepInstances()
            },
            new TestEntity()
            {
                Id = 4,
                Title = "Four",
                Description = "test 4.",
                OwningDepartment = Department.Trading,
                TestSteps = GetBasicTestStepInstances()
            }
        ];
    }

    private static List<TestStepEntity> GetBasicTestStepInstances()
    {
        return
        [
            new TestStepEntity()
            {
                StepPlacement = 1,
                Instructions = "Step one."
            },
            new TestStepEntity()
            {
                StepPlacement = 2,
                Instructions = "Step two."
            },
            new TestStepEntity()
            {
                StepPlacement = 3,
                Instructions = "Step three."
            },
            new TestStepEntity()
            {
                StepPlacement = 4,
                Instructions = "Step four."
            }
        ];
    }
}
