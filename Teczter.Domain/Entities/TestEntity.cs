using Teczter.Domain.Enums;
using Teczter.Domain.ValueObjects;

namespace Teczter.Domain.Entities;

public class TestEntity : BaseEntity
{
    private string _owningDepartment = Department.Unowned.ToString();

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
                throw new ArgumentException($"{value} is an invalid department.");
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
        TestSteps = TestSteps.OrderBy(x => x.StepPlacement).ToList();

        for (int i = 0; i < TestSteps.Count; i++)
        {
            TestSteps[i].StepPlacement = i + 1;
        }
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