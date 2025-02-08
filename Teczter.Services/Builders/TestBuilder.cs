using Teczter.Domain.Entities;
using Teczter.Domain.Exceptions;
using Teczter.Domain.ValueObjects;
using Teczter.Services.DTOs.Request;
using Teczter.Services.ServiceInterfaces;
using Teczter.Services.Validators.ValidatorAbstractions;

namespace Teczter.Services.Builders
{
    internal class TestBuilder(CzAbstractValidator<TestStepEntity> testStepValidator) : ITestBuilder
    {
        private readonly CzAbstractValidator<TestStepEntity> _testStepValidator = testStepValidator;

        private TestEntity _test = null!;

        public ITestBuilder AddLinkUrl(LinkUrl link)
        {
            _test.AddLinkUrl(link);
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
                    LinkUrls = step.LinkUrls
                };

                var validationResults = _testStepValidator.Validate(stepEntity);

                if (!validationResults[0].Success)
                {
                    throw new CzValidationException(ErrorMessageFormatter.CreateValidationErrorMessage(validationResults));
                }

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
                   LinkUrls = step.LinkUrls
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

        public ITestBuilder SetPillarOwner(string pillar)
        {
            _test.OwningPillar = pillar;
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

        public ITestBuilder AddLinkUrls(IEnumerable<LinkUrl> links)
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
