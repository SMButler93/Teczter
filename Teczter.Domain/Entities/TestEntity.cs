using Teczter.Domain.Entities.interfaces;
using Teczter.Domain.Enums;
using Teczter.Domain.Exceptions;
using Teczter.Domain.ValueObjects;

namespace Teczter.Domain.Entities;

public class TestEntity : IAuditableEntity, ISoftDeleteable, IHasIntId
{
    private string _owningDepartment = Department.Unowned.ToString();
    private int _id;

    public int Id => _id;
    public DateTime CreatedOn { get; } = DateTime.Now;
    public int CreatedById { get; set; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public bool IsDeleted { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<LinkUrl> LinkUrls { get; set; } = [];
    public string OwningDepartment

    {
        get => _owningDepartment;

        set
        {
            if (!ValidateOwningDepartment(value))
            {
                throw new TeczterValidationException($"{value} is an invalid department.");
            }

            _owningDepartment = value.ToUpper();
        }
    }

    public List<TestStepEntity> TestSteps { get; set; } = [];

    private static bool ValidateOwningDepartment(string department)
    {
        var validValues = Enum.GetNames(typeof(Department)).Select(x => x.ToLower());

        return validValues.Contains(department.ToLower());
    } 

    public void AddTestStep(TestStepEntity step)
    {
        TestSteps.Insert(step.StepPlacement - 1, step);
        SetCorrectStepPlacementValues();
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
    }

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

    public void AddLinkUrl(string linkUrl) => LinkUrls.Add(new LinkUrl(linkUrl));
    
    public void RemoveLinkUrl(LinkUrl linkUrl) => LinkUrls.Remove(linkUrl);

    public void Delete()
    {
        foreach (var step in TestSteps)
        {
            step.Delete();
        }

        IsDeleted = true;
    }
}