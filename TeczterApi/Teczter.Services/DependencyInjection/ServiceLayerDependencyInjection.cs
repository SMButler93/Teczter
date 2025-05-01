using Microsoft.Extensions.DependencyInjection;
using Teczter.Persistence;
using Teczter.Services.Builders;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.DependencyInjection;

public static class ServiceLayerDependencyInjection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        //Scoped services:
        services.AddScoped<ITestService, TestService>();
        services.AddScoped<ITestComposer, TestComposer>();
        services.AddScoped<IExecutionGroupService, ExecutionGroupService>();
        services.AddScoped<IExecutionGroupComposer, ExecutionGroupComposer>();
        services.AddScoped<IExecutionService, ExecutionService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}