using Teczter.Domain.Entities;
using Teczter.Domain.Enums;
using Teczter.Services.DTOs.Request;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.Builders
{
    internal class TestBuilder() : ITestBuilder
    {
        private TestEntity _test = null!;

        public ITestBuilder AddLinkUrl(string linkUrl)
        {
            _test.AddLinkUrl(linkUrl);
            return this;
        }

        public ITestBuilder AddSteps(IEnumerable<TestStepCommandRequestDto> steps)
        {
            var testStepEntities = new List<TestStepEntity>();

            foreach(var step in steps)
            {
                var stepEntity = new TestStepEntity
                {
                    TestId = _test.Id,
                    StepPlacement = step.StepPlacement,
                    Instructions = step.Instructions,
                    Urls = step.Urls
                };

                testStepEntities.Add(stepEntity);
            }

            _test.AddTestSteps(testStepEntities);

            return this;
        }

        public ITestBuilder AddStep(TestStepCommandRequestDto step)
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
            foreach(var step in steps)
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
            SetRevisonDetails();

            return this;
        }

        public ITestBuilder SetDescription(string description)
        {
            _test.Description = description;
            return this;
        }

        public ITestBuilder SetOwningDepartment(Department department)
        {
            _test.OwningDepartment = department;
            return this;
        }

        public ITestBuilder SetTitle(string title)
        {
            _test.Title = title;
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
}
