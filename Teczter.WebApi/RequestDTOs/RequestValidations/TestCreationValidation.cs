using FluentValidation;

namespace Teczter.WebApi.RequestDTOs.RequestValidations
{
    public class TestCreationValidation : AbstractValidator<TestCreationDto>
    {
        public TestCreationValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("A Title is Required.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("A Descriptions is Required.");

            RuleFor(x => x.OwningPillar)
                .NotEmpty()
                .WithMessage("You must specify the pillar that owns this test, or set to 'Unowned'.")
                .Must(IsValidPillar)
                .WithMessage("Please provide a valid pillar as the test owner.");
        }

        private bool IsValidPillar(string pillar)
        {
            string[] validPillars = { "ACCOUNTING", "CORE", "OPERATIONS", "TRADING", "UNOWNED" };

            return validPillars.Contains(pillar.ToUpper());
        }
    }
}
