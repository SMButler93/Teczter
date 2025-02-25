using FluentValidation;
using Teczter.Domain.Entities;
using Teczter.Domain.Exceptions;
using Teczter.Domain.ValueObjects;
using Teczter.Services.DTOs.Request;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.Builders
{
    internal class TestBuilder(IValidator<TestEntity> testValidator, IValidator<TestStepEntity> testStepValidator) : ITestBuilder
    {
        private readonly IValidator<TestEntity> _testValidator = testValidator;
        private readonly IValidator<TestStepEntity> _testStepValidator = testStepValidator;

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

                var stepValidationResults = _testStepValidator.Validate(stepEntity);

                if (!stepValidationResults.IsValid)
                {
                    var errorMessages = stepValidationResults.Errors.Select(x => x.ErrorMessage);
                    throw new TeczterValidationException(ErrorMessageBuilder.CreateValidationErrorMessage(errorMessages));
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
