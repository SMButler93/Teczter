using Microsoft.Extensions.DependencyInjection;
using Teczter.Domain.Entities;
using Teczter.Persistence;
using Teczter.Services.Builders;
using Teczter.Services.ServiceInterfaces;
using Teczter.Services.Validation.ValidationRules;
using Teczter.Services.Validation.ValidationRules.ValidationRulesProvider;
using Teczter.Services.Validation.Validators;

namespace Teczter.Services.DependencyInjection;

public static class ServiceLayerDependencyInjection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        //Scoped services:
        services.AddScoped<ITestService, TestService>();
        services.AddScoped<ITestStepService, TestStepService>();
        services.AddScoped<ITestBuilder, TestBuilder>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //Transient services:
        services.AddTransient(typeof(IValidator<>), typeof(Validator<>));
        services.AddTransient(typeof(AbstractValidationRulesProvider<TestEntity>), typeof(TestValidationRuleProvider));
        services.AddTransient(typeof(AbstractValidationRulesProvider<TestStepEntity>), typeof(TestStepValidationRuleProvider));
    }
}