using Teczter.Domain.Enums;
using Teczter.Domain.ValueObjects;

namespace Teczter.Domain.Entities;

public class TestEntity
{
    public Guid Id { get; } = new Guid();
    public bool IsDeleted { get; set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public Guid CreatedById { get; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public Guid RevisedById { get; set; }
    public Guid TestingRoundId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid? AssignedUserId { get; set; } = null;
    public List<LinkUrl> LinkUrls { get; set; } = [];
    public TestState TestState { get; private set; } = null!;
    public bool HasPassed => TestState.IsAPass;
    public bool HasBeenTested => !TestState.IsUntested;
    public Pillars PillarOwner { get; set; } = Pillars.Unowned;

    public List<TestStepEntity> TestSteps { get; set; } = [];
    public TestRoundEntity TestRound { get; set; } = null!;
    public UserEntity? AssignedUser { get; set; } = null;

    public void AddTestStep(TestStepEntity step)
    {
        TestSteps.Add(step);
    }

    public void RemoveTestStep(TestStepEntity step)
    {
        step.IsDeleted = true;
        TestSteps.Remove(step);
    }

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
        TestState.Pass();

        foreach (var step in TestSteps)
        {
            step.Pass();
        }
    }

    public void Fail()
    {
        TestState.Fail();
    }

    public void ResetTest()
    {
        TestState.Reset();

        foreach (var step in TestSteps)
        {
            step.Reset();
        }
    }
}