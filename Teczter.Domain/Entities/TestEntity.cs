using System.Text.RegularExpressions;
using Teczter.Domain.Entities.interfaces;
using Teczter.Domain.Enums;
using Teczter.Domain.Exceptions;

namespace Teczter.Domain.Entities;

public class TestEntity : IAuditableEntity, ISoftDeleteable, IHasIntId
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public int CreatedById { get; set; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public bool IsDeleted { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Urls { get; set; } = [];
    public Department OwningDepartment { get; set; }

    public List<TestStepEntity> TestSteps { get; set; } = [];

    public void AddTestStep(TestStepEntity step)
    {
        OrderTestSteps();
        TestSteps.Insert(step.StepPlacement - 1, step);
        SetCorrectStepPlacementValuesOnAdd();
    }

    public void RemoveTestStep(TestStepEntity step)
    {
        step.IsDeleted = true;
        TestSteps.Remove(step);
        SetCorrectStepPlacementValuesOnUpdate();
    }

    public void SetCorrectStepPlacementValuesOnUpdate()
    {
        OrderTestSteps();

        for (int i = 0; i < TestSteps.Count; i++)
        {
            TestSteps[i].StepPlacement = i + 1;
        }
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
        if (!IsValidUrl(url))
        {
            throw new TeczterValidationException($"{url} is an invalid URL.");
        }

        Urls.Add(url);
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

    private static bool IsValidUrl(string url)
    {
        var pattern = @"^(https?:\/\/www\.|www\.)[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(com|co\.uk|org)$";

        var regex = new Regex(pattern);

        return regex.IsMatch(url);
    }
}