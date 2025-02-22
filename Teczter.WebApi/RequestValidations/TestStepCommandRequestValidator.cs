using FluentValidation;
using Teczter.Services.DTOs.Request;

namespace Teczter.WebApi.RequestValidations
{
    public class TestStepCommandRequestValidator : AbstractValidator<TestStepCommandRequestDto>
    {
        public TestStepCommandRequestValidator()
        {

        }
    }
}
