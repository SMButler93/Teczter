﻿using Teczter.Domain.Entities;

namespace Teczter.WebApi.ResponseDtos;

public class TestStepDto
{
    public int Id { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedOn { get; } = DateTime.Now;
    public int CreatedById { get; }
    public DateTime RevisedOn { get; set; } = DateTime.Now;
    public int RevisedById { get; set; }
    public int TestId { get; set; }
    public int StepPlacement { get; set; }
    public string Instructions { get; set; } = null!;
    public List<string> LinkUrls { get; set; } = [];

    public TestStepDto(TestStepEntity testStep)
    {
        Id = testStep.Id;
        IsDeleted = testStep.IsDeleted;
        CreatedOn = testStep.CreatedOn;
        CreatedById = testStep.CreatedById;
        RevisedOn = testStep.RevisedOn;
        RevisedById = testStep.RevisedById;
        TestId = testStep.Id;
        StepPlacement = testStep.StepPlacement;
        Instructions = testStep.Instructions;
        LinkUrls = testStep.Urls;
    }
}
