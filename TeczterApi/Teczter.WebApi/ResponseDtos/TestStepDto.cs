using Teczter.Domain.Entities;

namespace Teczter.WebApi.ResponseDtos;

public class TestStepDto(TestStepEntity entity)
{
    public int Id { get; private set; } = entity.Id;
    public bool IsDeleted { get; set; } = entity.IsDeleted;
    public DateTime CreatedOn { get; } = entity.CreatedOn;
    public int CreatedById { get; } = entity.CreatedById;
    public DateTime RevisedOn { get; set; } = entity.RevisedOn;
    public int RevisedById { get; set; } = entity.RevisedById;
    public int TestId { get; set; } = entity.Id;
    public int StepPlacement { get; set; } = entity.StepPlacement;
    public string Instructions { get; set; } = entity.Instructions;
    public List<string> LinkUrls { get; set; } = entity.Urls;
}
