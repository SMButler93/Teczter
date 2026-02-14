using Teczter.Domain.Entities;

namespace Teczter.WebApi.ResponseDtos;

public class TestDto(TestEntity entity)
{
    public int Id { get; private set; } = entity.Id;
    public bool IsDeleted { get; set; } = entity.IsDeleted;
    public DateTime CreatedOn { get; } = entity.CreatedOn;
    public int CreatedById { get; } = entity.CreatedById;
    public DateTime RevisedOn { get; set; }  = entity.RevisedOn;
    public int RevisedById { get; set; }  = entity.RevisedById;
    public string Title { get; set; } = entity.Title;
    public string Description { get; set; }  = entity.Description;
    public List<string> LinkUrls { get; set; } = [.. entity.Urls];
    public string Department { get; set; } = entity.OwningDepartment.ToString();
    public List<TestStepDto> TestSteps { get; set; } = entity.TestSteps.Select(x => new TestStepDto(x)).ToList();
}