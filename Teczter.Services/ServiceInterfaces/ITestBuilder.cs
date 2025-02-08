using Teczter.Domain.Entities;
using Teczter.Domain.ValueObjects;
using Teczter.Services.DTOs.Request;

namespace Teczter.Services.ServiceInterfaces;

public interface ITestBuilder
{
    ITestBuilder NewInstance();
    ITestBuilder UsingContext(TestEntity test);
    ITestBuilder SetTitle(string title);
    ITestBuilder SetDescription(string description);
    ITestBuilder SetPillarOwner(string pillar);
    ITestBuilder AddSteps(IEnumerable<TestStepCommandRequestDto> steps);
    ITestBuilder AddStep(TestStepCommandRequestDto step);
    ITestBuilder AddSteps(IEnumerable<TestStepEntity> steps);
    ITestBuilder AddStep(TestStepEntity setp);
    ITestBuilder AddLinkUrls(IEnumerable<LinkUrl> links);
    ITestBuilder AddLinkUrl(LinkUrl link);
    TestEntity Build();
}
