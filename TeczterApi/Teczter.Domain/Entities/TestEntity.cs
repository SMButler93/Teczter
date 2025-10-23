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
    public byte[] RowVersion { get; set; } = [];
    public IReadOnlyList<string> Urls => _urls.AsReadOnly();

    private List<string> _urls = [];

    public virtual List<TestStepEntity> TestSteps { get; set; } = [];

    public void AddTestStep(TestStepEntity step)
    {
        OrderTestSteps();
        TestSteps.Insert(step.StepPlacement - 1, step);
        SetCorrectStepPlacementValuesOnAdd();
        RevisedOn = DateTime.Now;
    }

    public TeczterValidationResult<TestEntity> RemoveTestStep(int testStepId)
    {
        var testStep = TestSteps.SingleOrDefault(x => x.Id == testStepId && !x.IsDeleted);

        if (testStep is null)
        {
            return TeczterValidationResult<TestEntity>.Fail("Cannot remove a test step that does not exist, has already been deleted, or does not belong to this test.");
        }

        testStep.Delete();
        TestSteps.Remove(testStep);
        SetCorrectStepPlacementValuesOnUpdate();
        RevisedOn = DateTime.Now;

        return TeczterValidationResult<TestEntity>.Succeed(this);
    }

    public void SetCorrectStepPlacementValuesOnUpdate()
    {
        OrderTestSteps();

        for (int i = 0; i < TestSteps.Count; i++)
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

    public TeczterValidationResult<TestEntity> AddLinkUrl(string url)
    {
        if (!IsValidUrl(url))
        {
            return TeczterValidationResult<TestEntity>.Fail($"{url} is an invalid URL.");
        }

        if (Urls.Contains(url, StringComparer.OrdinalIgnoreCase))
        {
            return TeczterValidationResult<TestEntity>.Fail($"{url} already exists as a link for this test.");
        }

        _urls.Add(url);
        RevisedOn = DateTime.Now;

        return TeczterValidationResult<TestEntity>.Succeed(this);
    }

    public TeczterValidationResult<TestEntity> RemoveLinkUrl(string url)
    {
        if (!Urls.Contains(url, StringComparer.OrdinalIgnoreCase))
        {
            return TeczterValidationResult<TestEntity>.Fail($"{url} does not belong to this test");
        }

        _urls.Remove(url);
        RevisedOn = DateTime.Now;

        return TeczterValidationResult<TestEntity>.Succeed(this);
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

    public TeczterValidationResult<TestEntity> SetOwningDepartment(string department)
    {
        var isValidDepartment = Enum.TryParse<Department>(department, true, out var validDepartment);

        if (!isValidDepartment)
        {
            return TeczterValidationResult<TestEntity>.Fail($"{department} is an invalid department.");
        }

        OwningDepartment = validDepartment;

        return TeczterValidationResult<TestEntity>.Succeed(this);
    }

    private static bool IsValidUrl(string url)
    {
        var pattern = @"^(https?:\/\/www\.|www\.)[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(com|co\.uk|org)$";

        var regex = new Regex(pattern);

        return regex.IsMatch(url);
    }
}