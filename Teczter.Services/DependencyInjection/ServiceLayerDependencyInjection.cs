using Microsoft.Extensions.DependencyInjection;
using Teczter.Domain.Entities;
using Teczter.Persistence;
using Teczter.Services.Builders;
using Teczter.Services.ServiceInterfaces;
using Teczter.Services.ValidationServices;
using Teczter.Services.Validators;
using Teczter.Services.Validators.ValidatorAbstractions;

namespace Teczter.Services.DependencyInjection;

public static class ServiceLayerDependencyInjection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ITestService, TestService>();
        services.AddScoped<ITestStepService, TestStepService>();
        services.AddScoped<ITestBuilder, TestBuilder>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<CzAbstractValidator<TestEntity>, TestValidator>();
        services.AddScoped<CzAbstractValidator<TestStepEntity>, TestStepValidator>();
    }
}