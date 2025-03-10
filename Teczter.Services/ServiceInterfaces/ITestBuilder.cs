using Teczter.Domain.Entities;
using Teczter.Services.DTOs.Request;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestBuilder
{
    ITestBuilder NewInstance();
    ITestBuilder UsingContext(TestEntity test);
    ITestBuilder SetTitle(string title);
    ITestBuilder SetDescription(string description);
    ITestBuilder SetOwningDepartment(string department);
    ITestBuilder AddSteps(IEnumerable<TestStepCommandRequestDto> steps);
    ITestBuilder AddStep(TestStepCommandRequestDto step);
    ITestBuilder AddSteps(IEnumerable<TestStepEntity> steps);
    ITestBuilder AddStep(TestStepEntity setp);
    ITestBuilder AddLinkUrls(IEnumerable<string> links);
    ITestBuilder AddLinkUrl(string linkUrl);
    TestEntity Build();
}
