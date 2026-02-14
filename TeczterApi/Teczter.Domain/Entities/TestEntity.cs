using System.Text.RegularExpressions;
using Teczter.Domain.Entities.interfaces;
using Teczter.Domain.Enums;

namespace Teczter.Domain.Entities;

public class TestEntity : IAuditableEntity, ISoftDeleteable, IHasIntId
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; private set; } = DateTime.Now;
    public int CreatedById { get; set; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public bool IsDeleted { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Department OwningDepartment { get; set; }
    public List<string> Urls { get; set; } = [];
    public byte[] RowVersion { get; set; } = [];

    public virtual List<TestStepEntity> TestSteps { get; set; } = [];

    public void AddTestStep(TestStepEntity step)
    {
        OrderTestSteps();
        TestSteps.Insert(step.StepPlacement - 1, step);
        SetCorrectStepPlacementValuesOnAdd();
        OrderTestSteps();
        RevisedOn = DateTime.Now;
    }

    public void RemoveTestStep(int testStepId)
    {
        var testStep = TestSteps.SingleOrDefault(x => x.Id == testStepId && !x.IsDeleted);

        if (testStep is null) return;
        
        testStep.Delete();
        TestSteps.Remove(testStep);
        SetCorrectStepPlacementValuesOnUpdate();
        RevisedOn = DateTime.Now;
    }

    public void SetCorrectStepPlacementValuesOnUpdate()
    {
        OrderTestSteps();

        for (var i = 0; i < TestSteps.Count; i++)
        {
            TestSteps[i].StepPlacement = i + 1;
        }

        RevisedOn = DateTime.Now;
    }
    private void SetCorrectStepPlacementValuesOnAdd()
    {
        for (int i = 0; i < TestSteps.Count; i++)
        {
            TestSteps[i].StepPlacement = i + 1;
        }
    }

    public void OrderTestSteps()
    {
        TestSteps = TestSteps
            .OrderBy(x => x.StepPlacement)
            .ThenBy(x => x.RevisedOn)
            .ToList();
    }

    public void AddLinkUrl(string url)
    {
        Urls.Add(url);
        RevisedOn = DateTime.Now;
    }

    public void RemoveLinkUrl(string url)
    {
        if (!Urls.Contains(url, StringComparer.OrdinalIgnoreCase)) return;

        Urls.Remove(url);
        RevisedOn = DateTime.Now;
    }

    public void Delete()
    {
        foreach (var step in TestSteps)
        {
            step.Delete();
        }
        //Set revised by user ID.
        IsDeleted = true;
        RevisedOn = DateTime.Now;
    }

    public void SetOwningDepartment(string department)
    {
        OwningDepartment = Enum.Parse<Department>(department);
    }
}