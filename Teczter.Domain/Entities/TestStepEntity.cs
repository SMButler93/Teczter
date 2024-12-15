using Teczter.Domain.Enums;
using Teczter.Domain.ValueObjects;

namespace Teczter.Domain.Entities;

public class TestStepEntity
{
    public Guid Id { get; } = new Guid();
    public bool IsDeleted { get; set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public Guid CreatedById { get; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public Guid RevisedById { get; set; }
    public Guid TestId { get; set; }
    public int StepPlacement { get; set; }
    public string Instructions { get; set; } = null!;
    public List<LinkUrl> LinkUrls { get; set; } = [];
    public TestStateTypes State { get; private set; }
    public string? FailureReason { get; set; } = null;
    public bool HasPassed => State == TestStateTypes.Pass;
    public bool IsUntested => State == TestStateTypes.Untested;

    public TestEntity Test { get; set; } = null!;

    public void AddLinkUrl(LinkUrl linkUrl)
    {
        LinkUrls.Add(linkUrl);
    }

    public void RemoveLinkUrl(LinkUrl linkUrl)
    {
        LinkUrls.Remove(linkUrl);
    }

    public void Pass()
    {
        State = TestStateTypes.Pass;
    }

    public void Fail()
    {
        State = TestStateTypes.Fail;
    }

    public void Reset()
    {
        State = TestStateTypes.Untested;
    }
}
