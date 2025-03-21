using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Services.DTOs.Request;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestBuilder
{
    ITestBuilder NewInstance();
    ITestBuilder UsingContext(TestEntity test);
    ITestBuilder SetTitle(string title);
    ITestBuilder SetDescription(string description);
    ITestBuilder SetOwningDepartment(Department department);
    ITestBuilder AddSteps(IEnumerable<CreateTestStepRequestDto> steps);
    ITestBuilder AddStep(CreateTestStepRequestDto step);
    ITestBuilder AddSteps(IEnumerable<TestStepEntity> steps);
    ITestBuilder AddStep(TestStepEntity setp);
    ITestBuilder AddLinkUrls(IEnumerable<string> links);
    ITestBuilder AddLinkUrl(string linkUrl);
    TestEntity Build();
}
