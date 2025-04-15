using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Services.RequestDtos;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestComposer
{
    ITestComposer NewInstance();
    ITestComposer UsingContext(TestEntity test);
    ITestComposer SetTitle(string? title);
    ITestComposer SetDescription(string? description);
    ITestComposer SetOwningDepartment(string? department);
    ITestComposer AddSteps(IEnumerable<CreateTestStepRequestDto> steps);
    ITestComposer AddStep(CreateTestStepRequestDto step);
    ITestComposer AddSteps(IEnumerable<TestStepEntity> steps);
    ITestComposer AddStep(TestStepEntity setp);
    ITestComposer AddLinkUrls(IEnumerable<string> links);
    ITestComposer AddLinkUrl(string linkUrl);
    TestEntity Build();
}
