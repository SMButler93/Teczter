using Microsoft.Extensions.DependencyInjection;
using Teczter.Adapters.MiddlewareRepositories.ErrorLogRepository;
using Teczter.Adapters.MiddlewareRepositories.RequestLogRepository;
using Teczter.Adapters.ValidationRepositories;
using Teczter.Services.AdapterInterfaces;
using Teczter.Services.ValidationRepositoryInterfaces;

namespace Teczter.Adapters.DependencyInjection;

public static class AdapterLayerDependencyInjection
{
    public static void RegisterAdapters(this IServiceCollection services)
    {
        services.AddScoped<ITestAdapter, TestAdapter>();
        services.AddScoped<IExecutionGroupAdapter, ExecutionGroupAdapter>();
        services.AddScoped<IExecutionAdapter, ExecutionAdapter>();
        services.AddScoped<IUserAdapter, UserAdapter>();

        //Validation repositories
        services.AddScoped<ITestValidationRepository, TestValidationRepository>();
        services.AddScoped<IExecutionGroupValidationRepository, ExecutionGroupValidationRepository>();

        //MiddlewareRepositories
        services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
        services.AddScoped<IRequestLogRepository, RequestLogRepository>();
    }
}
