using Teczter.Domain.Entities;

namespace Teczter.WebApi.ResponseDtos;

public class TestDto
{
    public int Id { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedOn { get; }
    public int CreatedById { get; }
    public DateTime RevisedOn { get; set; }
    public int RevisedById { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<string> LinkUrls { get; set; } = [];
    public string Department { get; set; } = null!;

    public List<TestStepDto> TestSteps { get; set; } = [];

    public TestDto(TestEntity test)
    {
        Id = test.Id;
        IsDeleted = test.IsDeleted;
        CreatedOn = test.CreatedOn;
        CreatedById = test.CreatedById;
        RevisedOn = test.RevisedOn;
        RevisedById = test.RevisedById;
        Title = test.Title;
        Description = test.Description;
        LinkUrls = test.Urls;
        Department = test.OwningDepartment.ToString();
        TestSteps = test.TestSteps.Select(x => new TestStepDto(x)).ToList();
    }
}