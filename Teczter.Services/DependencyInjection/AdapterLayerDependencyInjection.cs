using Microsoft.Extensions.DependencyInjection;
using Teczter.Adapters;
using Teczter.Adapters.AdapterInterfaces;
using Teczter.Adapters.ValidationRepositories.ExecutionGroupValidationRepositories;
using Teczter.Adapters.ValidationRepositories.TestValidationRespositories;

namespace Teczter.Services.DependencyInjection;

public static class AdapterLayerDependencyInjection
{
    public static void RegisterAdapters(this IServiceCollection services)
    {
        services.AddScoped<ITestAdapter, TestAdapter>();
        services.AddScoped<IExecutionGroupAdapter, ExecutionGroupAdapter>();
        services.AddScoped<IExecutionAdapter, ExecutionAdapter>();
        services.AddScoped<IUserAdapter, UserAdapter>();

        //Validation repositories
        services.AddScoped<ITestValidationRepository, TestValidationRespository>();
        services.AddScoped<IExecutionGroupValidationRepository, ExecutionGroupValidationRepository>();
    }
}
