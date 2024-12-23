using Teczter.Domain.Entities;
using Teczter.Domain.ValueObjects;

namespace Teczter.WebApi.DTOs;

public class TestStepBasicDto
{
    public Guid Id { get; private set; }
    public Guid TestId { get; set; }
    public int StepPlacement { get; set; }
    public string Instructions { get; set; } = null!;
    public List<LinkUrl> LinkUrls { get; set; } = [];

    public TestStepBasicDto(TestStepEntity testStep)
    {
        Id = testStep.Id;
        TestId = testStep.Id;
        StepPlacement = testStep.StepPlacement;
        Instructions = testStep.Instructions;
        LinkUrls = testStep.LinkUrls;
    }
}

public class TestStepDetailedDto
{
    public Guid Id { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public int CreatedById { get; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public Guid TestId { get; set; }
    public int StepPlacement { get; set; }
    public string Instructions { get; set; } = null!;
    public List<LinkUrl> LinkUrls { get; set; } = [];

    public TestEntity Test { get; set; } = null!;

    public TestStepDetailedDto(TestStepEntity testStep)
    {
        Id = testStep.Id;
        IsDeleted = testStep.IsDeleted;
        CreatedOn = testStep.CreatedOn;
        CreatedById = testStep.CreatedById;
        RevisedOn = testStep.RevisedOn;
        RevisedById = testStep.RevisedById;
        TestId = testStep.Id;
        StepPlacement = testStep.StepPlacement;
        Instructions = testStep.Instructions;
        LinkUrls = testStep.LinkUrls;
        Test = testStep.Test;
    }
}
