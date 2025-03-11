using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Teczter.Data;
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
        services.AddScoped<ITestBuilder, TestBuilder>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}