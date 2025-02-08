using Teczter.Domain.Enums;
using Teczter.Domain.ValueObjects;

namespace Teczter.Domain.Entities;

public class TestEntity
{
    private string _owningPillar = Pillar.UNOWNED.ToString();

    public Guid Id { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public int CreatedById { get; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<LinkUrl> LinkUrls { get; set; } = [];
    public string OwningPillar
    {
        get
        {
            return _owningPillar;
        }
        set
        {
            if (!ValidateOwningPillar(value))
            {
                throw new ArgumentException($"{value} is an invalid pillar.");
            }

            _owningPillar = value.ToUpper();
        }
    }

    public List<TestStepEntity> TestSteps { get; set; } = [];

    private static bool ValidateOwningPillar(string pillar)
    {
        var validValues = Enum.GetNames(typeof(Pillar));

        return validValues.Contains(pillar.ToUpper());
    } 

    public void AddTestStep(TestStepEntity step)
    {
        TestSteps.Insert(step.StepPlacement - 1, step);
        SetCorrectStepPlacementValues();
        TestSteps.OrderBy(x => x.StepPlacement);
    }

    public void AddTestSteps(List<TestStepEntity> steps)
    {
        foreach(var step in steps)
        {
            AddTestStep(step);
        }
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

    public void Delete()
    {
        foreach (var step in TestSteps)
        {
            step.Delete();
        }

        IsDeleted = true;
    }
}