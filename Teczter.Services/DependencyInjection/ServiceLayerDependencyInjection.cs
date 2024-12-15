using Microsoft.Extensions.DependencyInjection;
using Teczter.Persistence;
using Teczter.Services.ServiceInterfaces;

namespace Teczter.Services.DependencyInjection;

public static class ServiceLayerDependencyInjection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ITestAdministrationService, TestAdministrationService>();
        services.AddScoped<ITestRoundService, TestRoundService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}