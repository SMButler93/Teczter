using Teczter.Domain.Enums;
using Teczter.Domain.ValueObjects;
using Teczter.Domain.Entities;

namespace Teczter.WebApi.DTOs;

public class TestDto
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid CreatedById { get; set; }
    public DateTime RevisedOn { get; set; }
    public Guid RevisedById { get; set; }
    public Guid TestingRoundId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid? AssignedToId { get; set; } = null;
    public List<LinkUrl> LinkUrls { get; set; } = [];
    public Pillar PillarOwner { get; set; }

    public List<TestStepEntity> TestSteps { get; set; } = [];
    public ExecutionGroupEntity TestRound { get; set; } = null!;
    public UserEntity? AssignedUser { get; set; }

    public TestDto(TestEntity test)
    {
        Id = test.Id;
        IsDeleted = test.IsDeleted;
        CreatedOn = test.CreatedOn;
        CreatedById = test.CreatedById;
        RevisedOn = test.RevisedOn;
        RevisedById = test.RevisedById;
    }
}