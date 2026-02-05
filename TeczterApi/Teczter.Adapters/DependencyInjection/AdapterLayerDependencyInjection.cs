using Microsoft.Extensions.DependencyInjection;
using Teczter.Services.AdapterInterfaces;

namespace Teczter.Adapters.DependencyInjection;

public static class AdapterLayerDependencyInjection
{
    public static void RegisterAdapters(this IServiceCollection services)
    {
        services.AddScoped<ITestAdapter, TestAdapter>();
        services.AddScoped<IExecutionGroupAdapter, ExecutionGroupAdapter>();
        services.AddScoped<IExecutionAdapter, ExecutionAdapter>();
    }
}
