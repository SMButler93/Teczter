using Teczter.Domain.ValueObjects;

namespace Teczter.Domain.Entities;

public class TestEntity
{
    private readonly string[] Pillars = { "ACCOUNTING", "CORE", "OPERATIONS", "TRADING", "UNOWNED" };
    private string _pillar = "UNOWNED";

    public Guid Id { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public Guid CreatedById { get; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public Guid RevisedById { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<LinkUrl> LinkUrls { get; set; } = [];

    public string Pillar
    {
        get
        {
            return _pillar;
        }
        set
        {
            if (!Pillars.Contains(value.ToUpper()))
            {
                throw new ArgumentException($"{value} is an invalid pillar. Valid options: Accounting, Core, Operations, Trading or Unowned.");
            }

            _pillar = value.ToUpper();
        }
    }

    public List<TestStepEntity> TestSteps { get; set; } = [];

    public void AddTestStep(TestStepEntity step)
    {
        TestSteps.Insert(step.StepPlacement - 1, step);
        SetCorrectStepPlacementValues();
        TestSteps.OrderBy(x => x.StepPlacement);
    }

    public void RemoveTestStep(TestStepEntity step)
    {
        step.IsDeleted = true;
        TestSteps.Remove(step);
        SetCorrectStepPlacementValues();
        TestSteps.OrderBy(x => x.StepPlacement);
    }

    private void SetCorrectStepPlacementValues()
    {
        for (int i = 0; i < TestSteps.Count; i++)
        {
            TestSteps[i].StepPlacement = i + 1;
        }
    }

    public void AddLinkUrl(LinkUrl linkUrl)
    {
        LinkUrls.Add(linkUrl);
    }

    public void RemoveLinkUrl(LinkUrl linkUrl)
    {
        LinkUrls.Remove(linkUrl);
    }
}