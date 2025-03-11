using Teczter.Domain.Entities.interfaces;
using Teczter.Domain.Enums;
using Teczter.Domain.Exceptions;

namespace Teczter.Domain.Entities;

public class TestEntity : IAuditableEntity, ISoftDeleteable, IHasIntId
{
    public int Id { get; private set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public int CreatedById { get; set; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public bool IsDeleted { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<string> Urls { get; set; } = [];
    public Department OwningDepartment { get; set; }

    public List<TestStepEntity> TestSteps { get; set; } = [];

    public void AddTestStep(TestStepEntity step)
    {
        TestSteps.Insert(step.StepPlacement - 1, step);
        SetCorrectStepPlacementValues();
    }

    public void AddTestSteps(List<TestStepEntity> steps)
    {
        foreach(var step in steps)
        {
            TestSteps.Add(step);
        }

        OrderTestSteps();
    }

    public void RemoveTestStep(TestStepEntity step)
    {
        step.IsDeleted = true;
        TestSteps.Remove(step);
        SetCorrectStepPlacementValues();
    }

    public void EnsureTestStepOrderingIsValidPostUpdate() => SetCorrectStepPlacementValues();

    private void SetCorrectStepPlacementValues()
    {
        OrderTestSteps();

        for (int i = 0; i < TestSteps.Count; i++)
        {
            TestSteps[i].StepPlacement = i + 1;
        }
    }

    private void OrderTestSteps()
    {
        TestSteps = TestSteps
            .OrderBy(x => x.StepPlacement)
            .ThenByDescending(x => x.RevisedOn)
            .ToList();
    }

    public void AddLinkUrl(string url)
    {
        if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var _))
        {
            Urls.Add(url);
        }
    }
    
    public void RemoveLinkUrl(string url) => Urls.Remove(url);

    public void Delete()
    {
        foreach (var step in TestSteps)
        {
            step.Delete();
        }

        IsDeleted = true;
    }
}