﻿using Teczter.Domain.Entities;
using Teczter.Domain.ValueObjects;

namespace Teczter.WebApi.ResponseDtos;

public class TestBasicDto
{
    public Guid Id { get; private set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Department { get; set; } = null!;

    public TestBasicDto(TestEntity test)
    {
        Id = test.Id;
        Title = test.Title;
        Description = test.Description;
        Department = test.OwningDepartment;
    }
}

public class TestDetailedDto
{
    public Guid Id { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public int CreatedById { get; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<LinkUrl> LinkUrls { get; set; } = [];
    public string Department { get; set; } = null!;

    public List<TestStepBasicDto> TestSteps { get; set; } = [];

    public TestDetailedDto(TestEntity test)
    {
        Id = test.Id;
        IsDeleted = test.IsDeleted;
        CreatedOn = test.CreatedOn;
        CreatedById = test.CreatedById;
        RevisedOn = test.RevisedOn;
        RevisedById = test.RevisedById;
        Title = test.Title;
        Description = test.Description;
        LinkUrls = test.LinkUrls;
        Department = test.OwningDepartment;
        TestSteps = test.TestSteps.Select(x => new TestStepBasicDto(x)).ToList();
    }
}