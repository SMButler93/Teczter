﻿using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Services.RequestDtos;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.Builders;

internal class TestBuilder() : ITestBuilder
{
    private TestEntity _test = null!;

    public ITestBuilder AddLinkUrl(string linkUrl)
    {
        _test.AddLinkUrl(linkUrl);
        return this;
    }

    public ITestBuilder AddSteps(IEnumerable<CreateTestStepRequestDto> steps)
    {
        var testStepEntities = new List<TestStepEntity>();

        foreach (var step in steps)
        {
            AddStep(step);
        }

        _test.OrderTestSteps();

        return this;
    }

    public ITestBuilder AddStep(CreateTestStepRequestDto step)
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

    public ITestBuilder AddSteps(IEnumerable<TestStepEntity> steps)
    {
        foreach (var step in steps)
        {
            _test.AddTestStep(step);
        }

        return this;
    }

    public ITestBuilder AddStep(TestStepEntity step)
    {
        _test.AddTestStep(step);
        return this;
    }

    public ITestBuilder NewInstance()
    {
        _test = new();
        return this;
    }

    public ITestBuilder SetDescription(string? description)
    {
        _test.Description = description ?? _test.Description;
        return this;
    }

    public ITestBuilder SetOwningDepartment(string? department)
    {
        if (department == null)
        {
            return this;
        }

        _test.OwningDepartment = Enum.Parse<Department>(department, true);
        return this;
    }

    public ITestBuilder SetTitle(string? title)
    {
        _test.Title = title ?? _test.Title;
        return this;
    }

    public ITestBuilder UsingContext(TestEntity test)
    {
        _test = test;
        SetRevisonDetails();

        return this;
    }

    public TestEntity Build()
    {
        return _test;
    }

    public ITestBuilder AddLinkUrls(IEnumerable<string> links)
    {
        foreach (var link in links)
        {
            _test.AddLinkUrl(link);
        }

        return this;
    }

    private void SetRevisonDetails()
    {
        _test.RevisedOn = DateTime.Now;
    }
}
