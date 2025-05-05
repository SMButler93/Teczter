using Teczter.Domain.Entities;
using Teczter.Services.RequestDtos.TestSteps;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestComposer
{
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
