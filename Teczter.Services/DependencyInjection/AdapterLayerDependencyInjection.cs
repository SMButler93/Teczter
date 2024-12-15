using Microsoft.Extensions.DependencyInjection;
using Teczter.Adapters;
using Teczter.Adapters.AdapterInterfaces;

namespace Teczter.Services.DependencyInjection;

public static class AdapterLayerDependencyInjection
{
    public static void RegisterAdapters(this IServiceCollection services)
    {
        services.AddScoped<ITestAdministrationAdapter, TestAdministrationAdapter>();
        services.AddScoped<ITestRoundAdapter, TestRoundAdapter>();
    }
}
