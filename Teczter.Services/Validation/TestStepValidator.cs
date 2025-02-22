using FluentValidation;
using Teczter.Domain.Entities;

namespace Teczter.Services.Validation;

public class TestStepValidator : AbstractValidator<TestStepEntity>
{
    public TestStepValidator()
    {

    }
}
