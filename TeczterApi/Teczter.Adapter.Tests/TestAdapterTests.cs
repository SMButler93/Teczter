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
        var ct = new CancellationTokenSource().Token;
        
        //Act:
        var result = await _sut.GetTestById(testToSearchFor.Id, ct);

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
        const int nonExistentTestId = 123;
        var ct = new CancellationTokenSource().Token;

        //Act:
        var result = await _sut.GetTestById(nonExistentTestId, ct);

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
        await _sut.AddNewTest(testToAdd, CancellationToken.None);
        await _dbContext.SaveChangesAsync();
        var persistedTestsWithNewEntry = await _dbContext.Tests.ToListAsync();

        //Assert:
        persistedTestsWithNewEntry.Count.ShouldBeGreaterThan(initialPersistedTestsCount);
        persistedTestsWithNewEntry.Select(x => x.Id).ShouldContain(testToAdd.Id);
    }

    [Test]
    public async Task GetBasicTestSearchResults_WhenNoAdditionalFiltersAreApplied_ShouldReturnUpTo15NonDeletedTests()
    {
        //Arrange:
        var preSeededTests = await _dbContext.Tests.ToListAsync();
        var testToMarkAsDeleted = preSeededTests.First();
        testToMarkAsDeleted.Delete();
        await _dbContext.SaveChangesAsync();

        //Act:
        var results = await _sut.GetTestSearchResults(1, null, null, CancellationToken.None);

        //Assert:
        results.Count.ShouldBeLessThanOrEqualTo(15);
        results.Count.ShouldBeLessThan(preSeededTests.Count);
        results.ShouldNotContain(x => x.Id == testToMarkAsDeleted.Id);
    }

    [Test]
    public async Task GetTestSearchResults_WhenNoFiltersApplied_ShouldReturnUpTo15NonDeletedTests()
    {
        //Arrange:
        var preSeededTests = await _dbContext.Tests
            .Where(x => !x.IsDeleted)
            .Take(15)
            .ToListAsync();
        
        var preSeededTestIds = preSeededTests.Select(x => x.Id).OrderBy(x => x).ToList();

        //Act:
        var results = await _sut.GetTestSearchResults(1, null, null, CancellationToken.None);

        //Assert:
        var resultsIds = results.Select(x => x.Id).OrderBy(x => x).ToList();
        resultsIds.ShouldBeEquivalentTo(preSeededTestIds);
        results.Count.ShouldBeLessThanOrEqualTo(15);
    }

    [Test]
    public async Task GetTestSearchResults_WhenFilteredOnTestName_ShouldGetInstancesThatMatch()
    {
        //Arrange:
        var preSeededTests = await _dbContext.Tests.ToListAsync();
        var expectedTest = preSeededTests.First();

        //Act:
        var results = await _sut.GetTestSearchResults(1, "One", null, CancellationToken.None);

        //Assert:
        results.Count.ShouldBe(1);
        results[0].Id.ShouldBe(expectedTest.Id);
    }

    [Test]
    public async Task GetTestSearchResults_WhenFilteredOnOwningDepartment_ShouldGetInstancesThatMatch()
    {
        //Arrange:
        var preSeededTests = await _dbContext.Tests.ToListAsync();
        var expectedTest = preSeededTests.First(x => x.OwningDepartment == Department.Accounting);

        //Act:
        var results = await _sut.GetTestSearchResults(1, null, nameof(Department.Accounting), CancellationToken.None);

        //Assert:
        results.Count.ShouldBe(1);
        results[0].Id.ShouldBe(expectedTest.Id);
    }

    [Test]
    public async Task GetTestSearchResults_WhenFilteredWithNoMatches_ShouldReturnEmptyList()
    {
        //Act:
        var results = await _sut.GetTestSearchResults(1, "ABC", null, CancellationToken.None);

        //Assert:
        results.ShouldBeEmpty();
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
