using Teczter.Domain;
using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos.TestSteps;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.ComposersAndBuilders;

public class TestComposer() : ITestComposer
{
    private TestEntity _test = new();
    private List<string> Errors = [];

    public ITestComposer AddLinkUrl(string linkUrl)
    {
        var result = _test.AddLinkUrl(linkUrl);

        if (!result.IsValid)
        {
            Errors.AddRange(result.ErrorMessages!);
        }

        return this;
    }

    public ITestComposer AddSteps(IEnumerable<CreateTestStepRequestDto> steps)
    {
        foreach (var step in steps)
        {
            AddStep(step);
        }

        return this;
    }

    public ITestComposer AddStep(CreateTestStepRequestDto step)
    {
        _test.AddTestStep(
           new TestStepEntity
           {
               TestId = _test.Id,
               StepPlacement = step.StepPlacement,
               Instructions = step.Instructions,
               Urls = step.Urls
           });

        return this;
    }

    public ITestComposer AddSteps(IEnumerable<TestStepEntity> steps)
    {
        foreach (var step in steps)
        {
            _test.AddTestStep(step);
        }

        return this;
    }

    public ITestComposer AddStep(TestStepEntity step)
    {
        _test.AddTestStep(step);
        return this;
    }

    public ITestComposer SetDescription(string? description)
    {
        _test.Description = description ?? _test.Description;

        if (_test.Description is null)
        {
            Errors.Add("A test must have a description.");
        }

        return this;
    }

    public ITestComposer SetOwningDepartment(string? department)
    {
        if (department is null)
        {
            return this;
        }

        var result = _test.SetOwningDepartment(department);

        if (!result.IsValid)
        {
            Errors.AddRange(result.ErrorMessages!);
        }

        return this;
    }

    public ITestComposer SetTitle(string? title)
    {
        _test.Title = title ?? _test.Title;

        if (string.IsNullOrWhiteSpace(_test.Title))
        {
            Errors.Add("A test must have a title.");
        }

        return this;
    }

    public ITestComposer UsingContext(TestEntity test)
    {
        _test = test;
        SetRevisionDetails();

        return this;
    }

    public TeczterValidationResult<TestEntity> Build()
    {
        if (Errors.Any())
        {
            return TeczterValidationResult<TestEntity>.Fail(Errors.ToArray());
        }

        return TeczterValidationResult<TestEntity>.Succeed(_test);
    }

    public ITestComposer AddLinkUrls(IEnumerable<string> links)
    {
        foreach (var link in links)
        {
            var result = _test.AddLinkUrl(link);

            if (!result.IsValid)
            {
                Errors.AddRange(result.ErrorMessages!);
            }
        }

        return this;
    }

    private void SetRevisionDetails()
    {
        _test.RevisedOn = DateTime.Now;
    }
}
